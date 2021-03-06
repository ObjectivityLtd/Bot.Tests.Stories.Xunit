﻿namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Dialogs;
    using Dialogs.State;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Services;
    using User;

    public class DemoBot : IBot
    {
        private const string DemoBotWelcomeMessage = "Welcome to demo bot";
        private readonly DemoUserStateAccessors demoUserStateAccessor;
        private readonly DialogSet dialogs;
        private readonly DemoDialogStateAccessors demoDialogStateAccessor;

        public DemoBot(
            DemoUserStateAccessors demoUserStateAccessor,
            DemoDialogStateAccessors demoDialogStateAccessor,
            IRoomService roomService)
        {
            this.demoUserStateAccessor = demoUserStateAccessor ?? throw new ArgumentNullException(nameof(demoUserStateAccessor));
            this.demoDialogStateAccessor = demoDialogStateAccessor ??
                                           throw new ArgumentNullException(nameof(demoDialogStateAccessor));

            this.dialogs = new DialogSet(demoDialogStateAccessor.DialogStateAccessor);
            this.dialogs.Add(new TestChoicePromptDialog(this.demoUserStateAccessor));
            this.dialogs.Add(new TestCardsDialog());
            this.dialogs.Add(new TestGreetingDialog(this.demoUserStateAccessor));
            this.dialogs.Add(new TestSumDialog(this.demoUserStateAccessor));
            this.dialogs.Add(new TestRoomDialog(roomService));
        }

        public async Task OnTurnAsync(
            ITurnContext turnContext,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await this.EnsureWelcomeMessage(turnContext, cancellationToken);

            var activity = turnContext.Activity;

            if (activity.Type == ActivityTypes.Message)
            {
                switch (activity.Text)
                {
                    case "hi":
                        await turnContext.SendActivityAsync($"You said: {activity.Text}", cancellationToken: cancellationToken);
                        break;
                    case "hello":
                        await this.HandleDialog(nameof(TestGreetingDialog), turnContext, cancellationToken);
                        break;
                    case "choice test":
                        await this.HandleDialog(nameof(TestChoicePromptDialog), turnContext, cancellationToken);
                        break;
                    case "cards test":
                        await this.HandleDialog(nameof(TestCardsDialog), turnContext, cancellationToken);
                        break;
                    case "sum test":
                        await this.HandleDialog(nameof(TestSumDialog), turnContext, cancellationToken);
                        break;
                    case "room test":
                        await this.HandleDialog(nameof(TestRoomDialog), turnContext, cancellationToken);
                        break;
                    case "token test":
                        await this.ShowTokenStatus(turnContext, cancellationToken);
                        break;
                    default:
                        var dc = await this.dialogs.CreateContextAsync(turnContext, cancellationToken);
                        await dc.ContinueDialogAsync(cancellationToken);
                        break;
                }

                await this.demoDialogStateAccessor.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
        }

        private async Task ShowTokenStatus(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Adapter is IUserTokenProvider tokenProvider)
            {
                var status = await tokenProvider.GetTokenStatusAsync(
                    turnContext,
                    turnContext.Activity.From.Id,
                    cancellationToken: cancellationToken);
                var tokenStatus = status?.FirstOrDefault();

                if (tokenStatus?.HasToken != null && tokenStatus.HasToken.Value)
                {
                    await turnContext.SendActivityAsync(
                        $"Token for connection {tokenStatus.ConnectionName} found",
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await turnContext.SendActivityAsync("No tokens found", cancellationToken: cancellationToken);
                }
            }
        }

        private async Task HandleDialog(string dialogId, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var dc = await this.dialogs.CreateContextAsync(turnContext, cancellationToken);
            var results = await dc.ContinueDialogAsync(cancellationToken);

            if (!dc.Context.Responded && results.Status == DialogTurnStatus.Empty && dc.ActiveDialog == null)
            {
                await dc.BeginDialogAsync(dialogId, null, cancellationToken);
            }
        }

        private async Task EnsureWelcomeMessage(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;

            if (activity.Type == ActivityTypes.ConversationUpdate && activity.MembersAdded != null)
            {
                foreach (var member in activity.MembersAdded)
                {
                    if (member.Id != activity.Recipient.Id)
                    {
                        await turnContext.SendActivityAsync(GetDemoBotWelcomeMessageForChannel(activity.ChannelId), cancellationToken: cancellationToken);
                    }
                }
            }

            await this.demoUserStateAccessor.UserState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        private string GetDemoBotWelcomeMessageForChannel(string channelId)
        {
            if (!String.Equals(channelId, "test", StringComparison.OrdinalIgnoreCase))
            {
                return $"{DemoBotWelcomeMessage} {channelId}";
            }

            return DemoBotWelcomeMessage;
        }
    }
}
