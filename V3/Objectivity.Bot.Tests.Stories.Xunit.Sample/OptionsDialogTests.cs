namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Dialogs;
    using global::Xunit;
    using Recorder;

    public class OptionsDialogTests : DialogUnitTestBase<OptionsDialog>
    {
        [Fact]
        public async Task SingleFruitStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Which fruit you take?")
                .Bot.ListsOptionsIncluding("Banana", "Apple", "Orange")
                .User.Says("Apple")
                .Bot.Says("Your choice: Apple")
                .Bot.Says("Which fruit you take now?")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task MultipleFruitsStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Which fruit you take?")
                .Bot.ListsOptionsIncluding("Banana", "Apple", "Orange")
                .User.Says("Apple")
                .Bot.Says("Your choice: Apple")
                .Bot.Says("Which fruit you take now?")
                .User.Says("Banana")
                .Bot.Says("Your choice: Banana")
                .Bot.Says("Which fruit you take now?")
                .User.Says("Orange")
                .Bot.Says("Your choice: Orange")
                .Bot.Says("Which fruit you take now?")
                .Rewind();

            await this.Play(story);
        }
    }
}
