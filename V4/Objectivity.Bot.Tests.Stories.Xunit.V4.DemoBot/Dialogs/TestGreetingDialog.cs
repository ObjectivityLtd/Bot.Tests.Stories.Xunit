namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using User;

    [Serializable]
    public class TestGreetingDialog : ComponentDialog
    {
        private readonly DemoUserStateAccessors userStateAccessor;

        private const string NamePrompt = nameof(NamePrompt);
        private const string AgePrompt = nameof(AgePrompt);
        private const string GreetingDialog = nameof(GreetingDialog);

        public TestGreetingDialog(DemoUserStateAccessors userStateAccessor)
            : base(nameof(TestGreetingDialog))
        {
            this.userStateAccessor = userStateAccessor;

            WaterfallStep[] steps = new WaterfallStep[]
            {
                this.PromptForNameAsync,
                this.PromptForAgeAsync,
                this.WelcomeUserAsync
            };

            this.AddDialog(new WaterfallDialog(GreetingDialog, steps));
            this.AddDialog(new TextPrompt(NamePrompt));
            this.AddDialog(new NumberPrompt<int>(AgePrompt));
        }

        private async Task<DialogTurnResult> PromptForNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                NamePrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What's your name?"),
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var name = stepContext.Result?.ToString();

            var userState = await this.userStateAccessor.DemoUserState.GetAsync(
                stepContext.Context,
                () => new DemoUserState(),
                cancellationToken: cancellationToken);

            userState.UserName = name;
            
            await this.userStateAccessor.UserState.SaveChangesAsync(stepContext.Context, cancellationToken: cancellationToken);

            return await stepContext.PromptAsync(
                AgePrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text($"How many floors have you climbed today?")
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> WelcomeUserAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var numberOfFloors = (int)stepContext.Result;

            var userState = await this.userStateAccessor.DemoUserState.GetAsync(
                stepContext.Context,
                () => new DemoUserState(),
                cancellationToken: cancellationToken);

            userState.FloorsPassed = numberOfFloors;

            if (numberOfFloors < 3)
            {
                await stepContext.Context.SendActivityAsync($"Come back when it's at least 3 {userState.UserName}.", cancellationToken: cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync($"Great score!", cancellationToken: cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}