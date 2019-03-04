namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    
    public class TestCardsDialog : ComponentDialog
    {
        private const string FruitDialogName = nameof(FruitDialogName);
        
        public TestCardsDialog() : base(nameof(TestCardsDialog))
        {
            var steps = new WaterfallStep[]
            {
                this.PromptForFruitAsync,
                this.AcknowledgeFruitAsync
            };
            
            this.AddDialog(new WaterfallDialog(FruitDialogName, steps));
        }

        private async Task<DialogTurnResult> PromptForFruitAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reply = stepContext.Context.Activity.CreateReply();
            
            var cardButtons = new[]
            {
                new CardAction
                {
                    Value = "Banana",
                    Title = "Banana",
                    Type = ActionTypes.ImBack
                },
                new CardAction
                {
                    Value = "Apple",
                    Title = "Apple",
                    Type = ActionTypes.ImBack
                },
                new CardAction
                {
                    Value = "Orange",
                    Title = "Orange",
                    Type = ActionTypes.ImBack
                }
            };

            var card = new HeroCard
            {
                Title = "Which fruit you take?",
                Buttons = cardButtons,
                Subtitle = "Choose one of our fruits"
            };

            var attachment = card.ToAttachment();

            reply.Attachments.Add(attachment);

            await stepContext.Context.SendActivityAsync(reply, cancellationToken: cancellationToken);
            
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private async Task<DialogTurnResult> AcknowledgeFruitAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = stepContext.Result?.ToString();

            if (!string.IsNullOrWhiteSpace(choice))
            {
                await stepContext.Context.SendActivityAsync($"Your choice: {choice}", cancellationToken: cancellationToken);
            }
            
            return await stepContext.EndDialogAsync(choice, cancellationToken: cancellationToken);
        }
    }
}