// Based on: https://raw.githubusercontent.com/Microsoft/BotBuilder/master/CSharp/Tests/Microsoft.Bot.Builder.Tests/DialogTaskTests.cs
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

namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using global::Xunit;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Connector;
    using Stories.Core;

    public abstract class DialogTestBase
    {
        public static IMessageActivity MakeTestMessage()
        {
            return new Activity
            {
                Id = Guid.NewGuid().ToString(),
                Type = ActivityTypes.Message,
                From = new ChannelAccount { Id = ChannelID.User },
                Conversation = new ConversationAccount { Id = Guid.NewGuid().ToString() },
                Recipient = new ChannelAccount { Id = ChannelID.Bot },
                ServiceUrl = "InvalidServiceUrl",
                ChannelId = "Test",
                Attachments = Array.Empty<Attachment>(),
                Entities = Array.Empty<Entity>(),
            };
        }

        public static async Task PostActivityAsync(ILifetimeScope container, IMessageActivity toBot, CancellationToken token)
        {
            using (var scope = DialogModule.BeginLifetimeScope(container, toBot))
            {
                var task = scope.Resolve<IPostToBot>();
                await task.PostAsync(toBot, token);
            }
        }

        public static async Task AssertScriptAsync(ILifetimeScope container, params string[] pairs)
        {
            Assert.NotEmpty(pairs);

            var toBot = MakeTestMessage();

            for (int index = 0; index < pairs.Length; ++index)
            {
                var toBotText = pairs[index];
                toBot.Text = toBotText;

                await PostActivityAsync(container, toBot, CancellationToken.None);

                var queue = container.Resolve<Queue<IMessageActivity>>();

                // if user has more to say, bot should have said something
                if (index + 1 < pairs.Length)
                {
                    Assert.NotEmpty(queue);
                }

                while (queue.Count > 0)
                {
                    ++index;

                    var toUser = queue.Dequeue();
                    string actual;
                    switch (toUser.Type)
                    {
                        case ActivityTypes.Message:
                            actual = toUser.Text;
                            break;
                        case ActivityTypes.EndOfConversation:
                            actual = toUser.AsEndOfConversationActivity().Code;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    var expected = pairs[index];

                    Assert.Equal(expected, actual);
                }
            }
        }

        public static void AssertMentions(string expectedText, IEnumerable<IMessageActivity> actualToUser)
        {
            var actualToUserList = actualToUser.ToList();

            Assert.NotEqual(1, actualToUserList.Count);

            var index = actualToUserList.Single().Text.IndexOf(expectedText, StringComparison.OrdinalIgnoreCase);

            Assert.True(index >= 0);
        }

        public static void AssertMentions(string expectedText, ILifetimeScope scope)
        {
            var queue = scope.Resolve<Queue<IMessageActivity>>();
            AssertMentions(expectedText, queue);
        }

        public static void AssertNoMessages(ILifetimeScope scope)
        {
            var queue = scope.Resolve<Queue<IMessageActivity>>();
            Assert.Empty(queue);
        }

        public static string NewID()
        {
            return Guid.NewGuid().ToString();
        }

#pragma warning disable 1998
        public static async Task AssertOutgoingActivity(ILifetimeScope container, Action<IMessageActivity> asserts)
#pragma warning restore 1998
        {
            var queue = container.Resolve<Queue<IMessageActivity>>();

            var toUser = queue.Dequeue();

            asserts(toUser);
        }
    }
}
