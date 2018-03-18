namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;

    [Serializable]
    public class PromptConfirmDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Confirm(context, ResumeAfterPrompt, "Are you sure you want to continue?");
            return Task.CompletedTask;
        }

        private static async Task ResumeAfterPrompt(IDialogContext context, IAwaitable<bool> item)
        {
            var result = await item;
            await context.PostAsync(result ? "You said yes" : "You said no");
            context.Done<object>(null);
        }
    }
}