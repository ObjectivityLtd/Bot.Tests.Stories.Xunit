namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot
{
    using System;
    using Dialogs;
    using Dialogs.State;
    using Microsoft.Bot.Builder.Dialogs;
    using User;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    public class DemoBot : IBot
    {
        private const string DemoBotWelcomeMessage = "Welcome to demo bot";
        private readonly DemoUserStateAccessors demoUserStateAccessor;
        private readonly DialogSet dialogs;
        private readonly DemoDialogStateAccessors demoDialogStateAccessor;

        public DemoBot(
            DemoUserStateAccessors demoUserStateAccessor,
            DemoDialogStateAccessors demoDialogStateAccessor)
        {
            this.demoUserStateAccessor = demoUserStateAccessor ?? throw new ArgumentNullException(nameof(demoUserStateAccessor));
            this.demoDialogStateAccessor = demoDialogStateAccessor ??
                                           throw new ArgumentNullException(nameof(demoDialogStateAccessor));
            
            this.dialogs = new DialogSet(demoDialogStateAccessor.DialogStateAccessor);
            this.dialogs.Add(new TestChoicePromptDialog());
            this.dialogs.Add(new TestCardsDialog());
            this.dialogs.Add(new TestGreetingDialog(this.demoUserStateAccessor));
            this.dialogs.Add(new TestSumDialog(this.demoUserStateAccessor));
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
                        await this.HandleDialog(nameof(TestChoicePromptDialog),turnContext, cancellationToken);
                        break;
                    case "cards test":
                        await this.HandleDialog(nameof(TestCardsDialog), turnContext, cancellationToken);
                        break;
                    case "sum test":
                        await this.HandleDialog(nameof(TestSumDialog), turnContext, cancellationToken);
                        break;
                    default:
                        var dc = await this.dialogs.CreateContextAsync(turnContext, cancellationToken);
                        await dc.ContinueDialogAsync(cancellationToken);
                        break;
                }

                await this.demoDialogStateAccessor.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
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
                        await turnContext.SendActivityAsync(DemoBotWelcomeMessage, cancellationToken: cancellationToken);
                    }
                }
            }

            await this.demoUserStateAccessor.UserState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }
    }
}
