namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Dialogs;
    using global::Xunit;
    using global::Xunit.Sdk;
    using Recorder;

    public class BotInitiatedLuisDialogTests : LuisDialogUnitTestBase<BotInitiatedLuisDialog>
    {
        public BotInitiatedLuisDialogTests()
        {
            this.RegisterUtterance("Play intent 1", x => x.Intent1(null, null));
            this.RegisterUtterance("Play intent 2", x => x.Intent2(null, null));
        }

        [Fact]
        public async Task AllIntentsStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Play intent 1")
                .Bot.Says("Intent 1")
                .User.Says("Play intent 2")
                .Bot.Says("Intent 2")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task SingleIntentStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Play intent 1")
                .Bot.Says("Intent 1")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task StoryWithWrongInitialActor_PlayStoryIsCalled_EqualExceptionThrown()
        {
            var story = StoryRecorder
                .Record()
                .User.Says("Play intent 1")
                .Bot.Says("Intent 1")
                .Rewind();

            await Assert.ThrowsAsync<TrueException>(async () =>
            {
                await this.Play(story);
            });
        }
    }
}