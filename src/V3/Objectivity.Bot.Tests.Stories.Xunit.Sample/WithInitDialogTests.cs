namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Extensions;
    using global::Xunit;
    using Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs;
    using Recorder;

    public class WithInitDialogTests : DialogUnitTestBase<WithInitDialog>
    {
        [Fact]
        public async Task WithInitDialog_InitExecuted()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Init executed: True")
                .DialogDone();

            this.RegisterDialog(x =>
            {
                var dialog = new WithInitDialog();
                dialog.Init();
                return dialog;
            });

            await this.Play(story);
        }

        [Fact]
        public async Task WithInitDialog_InitNotExecuted()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Init executed: False")
                .DialogDone();

            await this.Play(story);
        }
    }
}
