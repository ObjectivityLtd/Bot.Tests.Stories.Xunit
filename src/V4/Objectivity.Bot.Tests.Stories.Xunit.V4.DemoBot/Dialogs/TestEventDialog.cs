namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs.Prompts;

    public class TestEventDialog : ComponentDialog
    {
        private const string MainDialogId = nameof(MainDialogId);
        private const string AuthDialogId = nameof(AuthDialogId);

        public TestEventDialog()
            : base(nameof(TestEventDialog))
        {
            WaterfallStep[] steps =
            {
                this.RequestAuthorizationAsync,
                this.ShowAuthorizedAsync,
            };

            this.AddDialog(new WaterfallDialog(nameof(MainDialogId), steps));
            this.AddDialog(new TestAuthPrompt(nameof(AuthDialogId), this.TokenResponseValidatorAsync));

            this.InitialDialogId = nameof(MainDialogId);
        }

        protected async Task<bool> TokenResponseValidatorAsync(PromptValidatorContext<Activity> pc, CancellationToken cancellationToken)
        {
            var activity = pc.Recognized.Value;

            if (activity == null || activity.Type != ActivityTypes.Event)
            {
                await pc.Context.SendActivityAsync("You're unauthorized", cancellationToken: cancellationToken);

                return false;
            }

            return true;
        }

        private async Task<DialogTurnResult> RequestAuthorizationAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var authEventReply = stepContext.Context.Activity.CreateReply();
            authEventReply.Type = ActivityTypes.Event;
            authEventReply.Name = TestAuthPrompt.AuthRequestEventName;

            await stepContext.Context.SendActivityAsync(authEventReply, cancellationToken);

            return await stepContext.PromptAsync(nameof(AuthDialogId), new PromptOptions(), cancellationToken);
        }

        private async Task<DialogTurnResult> ShowAuthorizedAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("You're authorized", cancellationToken: cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}