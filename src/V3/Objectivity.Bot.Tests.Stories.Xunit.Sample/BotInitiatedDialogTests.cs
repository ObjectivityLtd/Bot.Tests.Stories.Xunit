namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Dialogs;
    using Extensions;
    using global::Xunit;
    using Recorder;

    public class BotInitiatedDialogTests : DialogUnitTestBase<BotInitiatedDialog>
    {
        [Fact]
        public async Task FullStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Hi")
                .Bot.Says("How are you?")
                .User.Says("I'm fine!")
                .DialogDone();

            await this.Play(story);
        }

        [Fact]
        public async Task PartialStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Hi")
                .Bot.Says("How are you?")
                .Rewind();

            await this.Play(story);
        }
    }
}
