namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;

    [Serializable]
    public class WrappedDialogResult
    {
        public WrappedDialogResult()
        {
            this.DialogStatus = DialogStatus.Idle;
        }

        public object Result { get; set; }

        public DialogStatus DialogStatus { get; set; }

        public Exception Exception { get; set; }
    }
}
