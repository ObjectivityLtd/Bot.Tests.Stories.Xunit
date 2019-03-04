namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using User;
    
    public class TestSumDialog : ComponentDialog
    {
        private const string MainDialogName = nameof(MainDialogName);
        private const string ChooseNumberDialogName = nameof(ChooseNumberDialogName);

        private readonly DemoUserStateAccessors userStateAccessor;

        public TestSumDialog(DemoUserStateAccessors userStateAccessor) : base(nameof(TestSumDialog))
        {
            this.userStateAccessor = userStateAccessor;

            var steps = new WaterfallStep[]
            {
                this.PromptForFirstNumberAsync,
                this.PromptForSecondNumberAsync,
                this.AcknowledgeNumberAsync
            };

            this.AddDialog(new WaterfallDialog(MainDialogName, steps));
            this.AddDialog(new NumberPrompt<int>(ChooseNumberDialogName, this.NumberValidatorAsync));
        }

        private async Task<DialogTurnResult> PromptForFirstNumberAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                ChooseNumberDialogName,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What's the first number?"),
                    RetryPrompt = MessageFactory.Text("A number must be between 0 and 100. Please try again.")
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForSecondNumberAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var firstNumber = int.Parse(stepContext.Result.ToString());

            var userState = await this.userStateAccessor.DemoUserState.GetAsync(
                stepContext.Context,
                () => new DemoUserState(),
                cancellationToken: cancellationToken);

            userState.PickedNumber = firstNumber;

            await this.userStateAccessor.UserState.SaveChangesAsync(stepContext.Context, cancellationToken: cancellationToken);

            return await stepContext.PromptAsync(
                ChooseNumberDialogName,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What's the second number?"),
                    RetryPrompt = MessageFactory.Text("A number must be between 0 and 100. Please try again.")
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgeNumberAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var secondNumber = int.Parse(stepContext.Result.ToString());

            var userState = await this.userStateAccessor.DemoUserState.GetAsync(
                stepContext.Context,
                () => new DemoUserState(),
                cancellationToken: cancellationToken);

            var sum = secondNumber + userState.PickedNumber;

            await stepContext.Context.SendActivityAsync($"Sum is: {sum.ToString(CultureInfo.InvariantCulture)}", cancellationToken: cancellationToken);

            return await stepContext.EndDialogAsync(sum, cancellationToken: cancellationToken);
        }

        public Task<bool> NumberValidatorAsync(PromptValidatorContext<int> promptContext, CancellationToken cancellationToken)
        {
            var number = promptContext.Recognized.Value;

            return Task.FromResult(promptContext.Recognized.Succeeded && number >= 0 && number <= 100);
        }
    }
}
