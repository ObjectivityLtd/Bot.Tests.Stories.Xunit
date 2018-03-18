namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;

    [Serializable]
    public class PromptChoiceDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context,
                ResumeAfterPrompt,
                new[] { "First", "Second", "Third" },
                "Choose one of the options");
            return Task.CompletedTask;
        }

        private static async Task ResumeAfterPrompt(IDialogContext context, IAwaitable<string> item)
        {
            var result = await item;
            await context.PostAsync("You chose " + result);
            context.Done<object>(null);
        }
    }
}