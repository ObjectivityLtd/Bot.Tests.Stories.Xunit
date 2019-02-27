namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Choices;
    using User;

    [Serializable]
    public class TestChoicePromptDialog : ComponentDialog
    {
        private readonly DemoUserStateAccessors userStateAccessor;

        private const string ColorPrompt = nameof(ColorPrompt);
        private const string FavoriteColorDialog = nameof(FavoriteColorDialog);
        private const string FruitPrompt = nameof(FruitPrompt);
        
        public TestChoicePromptDialog(DemoUserStateAccessors userStateAccessor) 
            : base(nameof(TestChoicePromptDialog))
        {
            this.userStateAccessor = userStateAccessor;

            WaterfallStep[] steps = {
                this.PromptForFavoriteColorAsync,
                this.PromptForFavoriteFruitAsync,
                this.AcknowledgeFavoriteColorAsync
            };

            this.AddDialog(new WaterfallDialog(FavoriteColorDialog, steps));
            this.AddDialog(new ChoicePrompt(ColorPrompt) { Style = ListStyle.SuggestedAction });
            this.AddDialog(new ChoicePrompt(FruitPrompt) { Style = ListStyle.Inline });
        }

        private async Task<DialogTurnResult> PromptForFavoriteColorAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                ColorPrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please choose your favorite color."),
                    RetryPrompt = MessageFactory.Text("Sorry, please choose a color from the list."),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Red", "Green", "Blue" })
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForFavoriteFruitAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = stepContext.Result as FoundChoice;

            var userState = await this.userStateAccessor.DemoUserState.GetAsync(
                stepContext.Context,
                () => new DemoUserState(),
                cancellationToken: cancellationToken);

            userState.FavoriteColor = choice?.Value;

            await this.userStateAccessor.UserState.SaveChangesAsync(stepContext.Context, cancellationToken: cancellationToken);
            
            return await stepContext.PromptAsync(
                FruitPrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please choose your favorite fruit."),
                    RetryPrompt = MessageFactory.Text("Sorry, please choose a fruit from the list."),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Apple", "Banana", "Orange" })
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgeFavoriteColorAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = stepContext.Result as FoundChoice;

            var userState = await this.userStateAccessor.DemoUserState.GetAsync(
                stepContext.Context,
                () => new DemoUserState(),
                cancellationToken: cancellationToken);

            if (choice != null)
            {
                await stepContext.Context.SendActivityAsync($"Your favorite fruit is {choice.Value} and color is {userState.FavoriteColor}", cancellationToken: cancellationToken);
            }

            return await stepContext.EndDialogAsync(choice, cancellationToken);
        }
    }
}