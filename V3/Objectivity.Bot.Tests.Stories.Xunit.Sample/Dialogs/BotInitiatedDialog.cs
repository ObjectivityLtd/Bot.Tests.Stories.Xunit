namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class BotInitiatedDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi");

            context.Wait(this.ResumeAfterFirstResponse);
        }

        private async Task ResumeAfterFirstResponse(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            await context.PostAsync("How are you?");
            context.Wait(this.ResumeAfterLastResponse);
        }

        private Task ResumeAfterLastResponse(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            context.Done<object>(null);
            return Task.CompletedTask;
        }
    }
}
