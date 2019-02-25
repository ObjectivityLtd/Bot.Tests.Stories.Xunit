namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Dialogs;
    using global::Xunit;
    using global::Xunit.Sdk;
    using Recorder;

    public class UserInitiatedDialogTests : DialogUnitTestBase<UserInitiatedDialog>
    {
        [Fact]
        public async Task FullStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .User.Says("hi")
                .Bot.Says("You said: hi")
                .User.Says("hello")
                .Bot.Says("You said: hello")
                .DialogDone();

            await this.Play(story);
        }

        [Fact]
        public async Task PartialStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .User.Says("hi")
                .Bot.Says("You said: hi")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task StoryWithWrongInitialActor_PlayStoryIsCalled_EqualExceptionThrown()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("hi")
                .User.Says("hi")
                .Rewind();

            await Assert.ThrowsAsync<TrueException>(async () =>
            {
                await this.Play(story);
            });
        }
    }
}
