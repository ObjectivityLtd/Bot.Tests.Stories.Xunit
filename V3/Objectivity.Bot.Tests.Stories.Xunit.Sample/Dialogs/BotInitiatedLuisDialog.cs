namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;

    [Serializable]
    public class BotInitiatedLuisDialog : LuisDialog<object>
    {
        public BotInitiatedLuisDialog(ILuisService service)
            : base(service)
        {
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Intent1")]
        public async Task Intent1(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Intent 1");
        }

        [LuisIntent("Intent2")]
        public async Task Intent2(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Intent 2");
        }
    }
}
