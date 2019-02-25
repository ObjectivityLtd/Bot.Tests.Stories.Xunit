namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;

    [Serializable]
    public class UserInitiatedLuisDialog : LuisDialog<object>
    {
        public UserInitiatedLuisDialog(ILuisService service)
            : base(service)
        {
        }

        public override Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceived);
            return Task.CompletedTask;
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
