namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dialogs;
    using Microsoft.Bot.Builder.Dialogs;

    public class DialogProxy : Dialog
    {
        private readonly Dialog baseDialog;
        private readonly WrappedDialogResult dialogResult;

        public DialogProxy(
            Dialog baseDialog,
            WrappedDialogResult result) : base(nameof(DialogProxy))
        {
            this.baseDialog = baseDialog;
            this.dialogResult = result;
        }

        public override async Task<DialogTurnResult> BeginDialogAsync(
            DialogContext dc,
            object options = null,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return this.CaptureResult(await this.baseDialog.BeginDialogAsync(dc, cancellationToken));
        }

        public override async Task<DialogTurnResult> ContinueDialogAsync(
            DialogContext dc,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return this.CaptureResult(await this.baseDialog.ContinueDialogAsync(dc, cancellationToken));
        }

        private DialogTurnResult CaptureResult(DialogTurnResult result)
        {
            this.dialogResult.Result = result.Result;

            switch (result.Status)
            {
                case DialogTurnStatus.Empty:
                    this.dialogResult.DialogStatus = DialogStatus.Idle;
                    break;
                case DialogTurnStatus.Waiting:
                    this.dialogResult.DialogStatus = DialogStatus.InProgress;
                    break;
                case DialogTurnStatus.Complete:
                    this.dialogResult.DialogStatus = DialogStatus.Finished;
                    break;
                case DialogTurnStatus.Cancelled:
                    this.dialogResult.DialogStatus = DialogStatus.Finished;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}