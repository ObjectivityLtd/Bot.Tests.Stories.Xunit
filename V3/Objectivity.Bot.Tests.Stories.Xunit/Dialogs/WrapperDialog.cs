namespace Objectivity.Bot.Tests.Stories.Xunit.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    internal class WrapperDialog : IDialog<object>
    {
        private readonly IDialog<object> wrappedDialog;
        private readonly WrappedDialogResult wrappedDialogResult;

        public WrapperDialog(IDialog<object> wrappedDialog, WrappedDialogResult wrappedDialogResult)
        {
            this.wrappedDialog = wrappedDialog;
            this.wrappedDialogResult = wrappedDialogResult;

            this.wrappedDialogResult.DialogStatus = DialogStatus.Idle;
        }

        public async Task StartAsync(IDialogContext context)
        {
            this.wrappedDialogResult.DialogStatus = DialogStatus.InProgress;

            await context.PostAsync(Consts.WrapperStartMessage);

            context.Wait(this.ResumeAfterStart);
        }

        private Task ResumeAfterStart(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            try
            {
                context.Call(this.wrappedDialog, this.ResumeAfterFinish);
            }
            catch
            {
                this.wrappedDialogResult.DialogStatus = DialogStatus.Failed;
            }

            return Task.CompletedTask;
        }

        private async Task ResumeAfterFinish(IDialogContext context, IAwaitable<object> item)
        {
            var result = await item;

            this.wrappedDialogResult.Result = result;

            if (this.wrappedDialogResult.DialogStatus != DialogStatus.Failed)
            {
                this.wrappedDialogResult.DialogStatus = DialogStatus.Finished;
            }
        }
    }
}
