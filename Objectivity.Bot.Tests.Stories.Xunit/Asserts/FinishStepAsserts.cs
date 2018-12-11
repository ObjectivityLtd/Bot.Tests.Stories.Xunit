namespace Objectivity.Bot.Tests.Stories.Xunit.Asserts
{
    using System;
    using System.Globalization;
    using Core;
    using global::Xunit;
    using StoryPlayer;

    public class FinishStepAsserts
    {
        private const string WrongExceptionTypeMessageFormat = "Expected dialog fail with exception of type = '{0}', actual exception type = '{1}'";
        private const string NoExceptionMessageFormat = "Expected dialog fail with exception of type = '{0}', but no exception was threw.";
        private const string NotEqualDialogStatusMessageFormat = "Expected dialog status = '{0}', actual status = '{1}'";
        private const string NotEqualDialogResultMessageFormat = "Dialog result = '{0}' doesn't match test predicate.";
        private const string WrongDialogResultTypeMessageFormat = "Dialog result = '{0}' is not of an expected type.";
        private const string ResultEmptyMessage = "Couldn't check result predicate - result is null.";

        private readonly WrappedDialogResult dialogResult;

        public FinishStepAsserts(WrappedDialogResult dialogResult)
        {
            this.dialogResult = dialogResult;
        }

        public void AssertDialogFinishStep(StoryStep storyStep)
        {
            var dialogStoryFrame = storyStep.StoryFrame as DialogStoryFrame;
            if (dialogStoryFrame == null)
            {
                return;
            }

            this.VerifyStatusesEqual(dialogStoryFrame);
            this.VerifyResultNotEmpty(dialogStoryFrame);

            if (dialogStoryFrame.ResultPredicate != null)
            {
                this.VerifyResultPredicate(dialogStoryFrame);
            }

            if (dialogStoryFrame.ExceptionType != null)
            {
                this.VerifyExceptionType(dialogStoryFrame);
            }
        }

        private void VerifyExceptionType(DialogStoryFrame dialogStoryFrame)
        {
            if (this.dialogResult.Exception == null)
            {
                var noExceptionMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    NoExceptionMessageFormat,
                    dialogStoryFrame.ExceptionType.Name);

                Assert.True(false, noExceptionMessage);
            }

            var exceptionType = this.dialogResult.Exception.GetType();

            var wrongExceptionTypeMessage = string.Format(
                CultureInfo.CurrentCulture,
                WrongExceptionTypeMessageFormat,
                dialogStoryFrame.ExceptionType.Name,
                exceptionType.Name);

            Assert.True(
                this.dialogResult.Exception.GetType() == dialogStoryFrame.ExceptionType,
                wrongExceptionTypeMessage);
        }

        private void VerifyResultPredicate(DialogStoryFrame dialogStoryFrame)
        {
            try
            {
                var notEqualResultMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    NotEqualDialogResultMessageFormat,
                    this.dialogResult.Result);

                Assert.True(dialogStoryFrame.ResultPredicate(this.dialogResult.Result), notEqualResultMessage);
            }
            catch (InvalidCastException)
            {
                var wrongDialogResultTypeMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    WrongDialogResultTypeMessageFormat,
                    this.dialogResult.Result);

                Assert.True(false, wrongDialogResultTypeMessage);
            }
        }

        private void VerifyResultNotEmpty(DialogStoryFrame dialogStoryFrame)
        {
            if (dialogStoryFrame.ResultPredicate != null && this.dialogResult.Result == null)
            {
                Assert.True(false, ResultEmptyMessage);
            }
        }

        private void VerifyStatusesEqual(DialogStoryFrame dialogStoryFrame)
        {
            var notEqualStatusesMessage = string.Format(
                            CultureInfo.CurrentCulture,
                            NotEqualDialogStatusMessageFormat,
                            dialogStoryFrame.DialogStatus,
                            this.dialogResult.DialogStatus);

            Assert.True(dialogStoryFrame.DialogStatus == this.dialogResult.DialogStatus, notEqualStatusesMessage);
        }
    }
}
