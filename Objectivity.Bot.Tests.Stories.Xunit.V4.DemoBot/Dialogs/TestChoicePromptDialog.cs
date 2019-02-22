namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Choices;

    [Serializable]
    public class TestChoicePromptDialog : ComponentDialog
    {
        private const string ColorPrompt = nameof(ColorPrompt);
        private const string FavoriteColorDialog = nameof(FavoriteColorDialog);
        
        public TestChoicePromptDialog() 
            : base(nameof(TestChoicePromptDialog))
        {
            WaterfallStep[] steps = new WaterfallStep[]
            {
                this.PromptForFavoriteColorAsync,
                this.AcknowledgeFavoriteColorAsync
            };

            this.AddDialog(new WaterfallDialog(FavoriteColorDialog, steps));
            this.AddDialog(new ChoicePrompt(ColorPrompt));
        }

        private async Task<DialogTurnResult> PromptForFavoriteColorAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                ColorPrompt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please choose your favorite color."),
                    RetryPrompt = MessageFactory.Text("Sorry, please choose a color from the list."),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "Red", ", Green", "Blue" }),
                },
                cancellationToken);
        }
        
        private async Task<DialogTurnResult> AcknowledgeFavoriteColorAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = stepContext.Result as FoundChoice;

            if (choice != null)
            {
                await stepContext.Context.SendActivityAsync($"Your favorite color is {choice.Value}", cancellationToken: cancellationToken);
            }

            return await stepContext.EndDialogAsync(choice, cancellationToken);
        }
    }
}