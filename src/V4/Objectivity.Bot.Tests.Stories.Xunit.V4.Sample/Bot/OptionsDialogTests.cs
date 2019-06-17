namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using global::Xunit;

    public class OptionsDialogTests : DemoBotTestBase
    {
        [Fact]
        public async Task SingleFruitStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = this.Record
                .User.Says("cards test")
                .Bot.ListsOptions("Which fruit you take?", new[] { "Banana", "Apple", "Orange" })
                .User.Says("Apple")
                .Bot.Says("Your choice: Apple")
                .Rewind();

            await this.Play(story);
        }
    }
}
