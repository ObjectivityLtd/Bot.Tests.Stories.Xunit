// Based on: https://github.com/Microsoft/BotBuilder-Azure/blob/master/CSharp/Tests/Microsoft.Bot.Builder.Azure.Tests/MockConnectorFactory.cs
//
//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Bot Framework: http://botframework.com
//
// Bot Builder SDK Github:
// https://github.com/Microsoft/BotBuilder
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// Only because we base on Microsoft's implementation where stateClient is obsolete we need to disabled warning

#pragma warning disable 612
#pragma warning disable 618
namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Connector;
    using Microsoft.Rest;
    using Moq;

    public class MockConnectorFactory : IConnectorClientFactory
    {
        private readonly IBotDataStore<BotData> memoryDataStore = new InMemoryDataStore();
        private readonly string botId;
        private StateClient stateClient;

        public MockConnectorFactory(string botId)
        {
            SetField.NotNull(out this.botId, nameof(botId), botId);
        }

        public Mock<StateClient> MockIBots(MockConnectorFactory mockConnectorFactory)
        {
            var botsClient = new Mock<StateClient>(MockBehavior.Loose);

            botsClient.Setup(d => d.BotState.SetConversationDataWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BotData>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, BotData, Dictionary<string, List<string>>, CancellationToken>(async (channelId, conversationId, data, headers, token)
                    => await mockConnectorFactory.UpsertData(channelId, null, conversationId, BotStoreType.BotConversationData, data));

            botsClient.Setup(d => d.BotState.GetConversationDataWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, Dictionary<string, List<string>>, CancellationToken>(async (channelId, conversationId, headers, token)
                    => await mockConnectorFactory.GetData(channelId, null, conversationId, BotStoreType.BotConversationData));

            botsClient.Setup(d => d.BotState.SetUserDataWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BotData>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
              .Returns<string, string, BotData, Dictionary<string, List<string>>, CancellationToken>(async (channelId, userId, data, headers, token)
                    => await mockConnectorFactory.UpsertData(channelId, userId, null, BotStoreType.BotUserData, data));

            botsClient.Setup(d => d.BotState.GetUserDataWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
                .Returns<string, string, Dictionary<string, List<string>>, CancellationToken>(async (channelId, userId, headers, token)
                    => await mockConnectorFactory.GetData(channelId, userId, null, BotStoreType.BotUserData));

            botsClient.Setup(d => d.BotState.SetPrivateConversationDataWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<BotData>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
             .Returns<string, string, string, BotData, Dictionary<string, List<string>>, CancellationToken>(async (channelId, conversationId, userId, data, headers, token)
                    => await mockConnectorFactory.UpsertData(channelId, userId, conversationId, BotStoreType.BotPrivateConversationData, data));

            botsClient.Setup(d => d.BotState.GetPrivateConversationDataWithHttpMessagesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, List<string>>>(), It.IsAny<CancellationToken>()))
             .Returns<string, string, string, Dictionary<string, List<string>>, CancellationToken>(async (channelId, conversationId, userId, headers, token) => await mockConnectorFactory.GetData(channelId, userId, conversationId, BotStoreType.BotPrivateConversationData));

            return botsClient;
        }

        public IConnectorClient MakeConnectorClient()
        {
            var client = new Mock<ConnectorClient> { CallBase = true };
            return client.Object;
        }

        public IStateClient MakeStateClient()
        {
            return this.stateClient ?? (this.stateClient = this.MockIBots(this).Object);
        }

        public IOAuthClient MakeOAuthClient()
        {
            throw new NotSupportedException();
        }

        protected IAddress AddressFrom(string channelId, string userId, string conversationId)
        {
            return new Address(
                this.botId,
                channelId,
                userId ?? "AllUsers",
                conversationId ?? "AllConversations",
                "InvalidServiceUrl");
        }

        protected async Task<HttpOperationResponse<BotData>> UpsertData(string channelId, string userId, string conversationId, BotStoreType storeType, BotData data)
        {
            var result = new HttpOperationResponse<BotData> { Request = new HttpRequestMessage() };
            try
            {
                var address = this.AddressFrom(channelId, userId, conversationId);
                await this.memoryDataStore.SaveAsync(address, storeType, data, CancellationToken.None);
            }
            catch (HttpException)
            {
                result.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.PreconditionFailed };
                return result;
            }
            catch (Exception)
            {
                result.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError };
                return result;
            }

            result.Body = data;
            result.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            return result;
        }

        protected async Task<HttpOperationResponse<BotData>> GetData(string channelId, string userId, string conversationId, BotStoreType storeType)
        {
            var result = new HttpOperationResponse<BotData> { Request = new HttpRequestMessage() };
            var address = this.AddressFrom(channelId, userId, conversationId);
            var data = await this.memoryDataStore.LoadAsync(address, storeType, CancellationToken.None);
            result.Body = data;
            result.Response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            return result;
        }
    }
}
