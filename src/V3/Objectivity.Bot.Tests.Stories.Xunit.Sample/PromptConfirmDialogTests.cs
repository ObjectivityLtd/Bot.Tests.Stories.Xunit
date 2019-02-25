namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Dialogs;
    using Extensions;
    using global::Xunit;
    using Recorder;

    public class PromptConfirmDialogTests : DialogUnitTestBase<PromptConfirmDialog>
    {
        [Fact]
        public async Task DialogWithConfirm_UserSelectsYes_BotSaysYouSaidYes()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Confirms("Are you sure you want to continue?")
                .User.Says("Yes")
                .Bot.Says("You said yes")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task DialogWithConfirm_UserSelectsNo_BotSaysYouSaidNo()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Confirms("Are you sure you want to continue?")
                .User.Says("No")
                .Bot.Says("You said no")
                .Rewind();

            await this.Play(story);
        }
    }
}