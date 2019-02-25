namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using global::Xunit;

    public class TestCardsDialogTests : DialogTestBase<TestCardsDialog>
    {
        [Fact]
        public async Task SingleFruitStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = this.Record
                .Bot.ListsOptions("Which fruit you take?", "Choose one of our fruits", new [] { "Banana", "Apple", "Orange" })
                .User.Says("Apple")
                .Bot.Says("Your choice: Apple")
                .Rewind();

            await this.Play(story);
        }
    }
}
