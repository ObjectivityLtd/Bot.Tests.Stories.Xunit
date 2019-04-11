namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;

    [Serializable]
    public class WithInitDialog : IDialog<object>
    {
        private bool initDone;

        public void Init()
        {
            this.initDone = true;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Init executed: {this.initDone}");
            context.Done(true);
        }
    }
}
