namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Services;

    public class TestRoomDialog : ComponentDialog
    {
        private const string MainDialogName = nameof(MainDialogName);
        private const string ChooseRoomNumberDialogName = nameof(ChooseRoomNumberDialogName);

        private readonly IRoomService roomService;

        public TestRoomDialog(IRoomService roomService) : base(nameof(TestRoomDialog))
        {
            this.roomService = roomService;

            var steps = new WaterfallStep[]
            {
                this.PromptForRoomNumberAsync,
                this.AcknowledgeRoomNumberAsync
            };

            this.AddDialog(new WaterfallDialog(MainDialogName, steps));
            this.AddDialog(new TextPrompt(ChooseRoomNumberDialogName, this.RoomNumberValidatorAsync));
        }

        private async Task<DialogTurnResult> PromptForRoomNumberAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                ChooseRoomNumberDialogName,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What's the room number?"),
                    RetryPrompt = MessageFactory.Text("A number must consist of three digits separated by a dot (ex. 1.23). Please try again.")
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgeRoomNumberAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var floor = this.roomService.GetRoomFloorByNumber(Convert.ToDecimal(stepContext.Result.ToString(), new NumberFormatInfo { NumberDecimalSeparator = "." }));

            await stepContext.Context.SendActivityAsync($"Room floor is: {floor}", cancellationToken: cancellationToken);

            return await stepContext.EndDialogAsync(floor, cancellationToken: cancellationToken);
        }

        public Task<bool> RoomNumberValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var roomNumber = promptContext.Recognized.Value;

            var rx = new Regex(@"^(\d\.\d{2})*$");

            return Task.FromResult(promptContext.Recognized.Succeeded && rx.IsMatch(roomNumber));
        }
    }
}
