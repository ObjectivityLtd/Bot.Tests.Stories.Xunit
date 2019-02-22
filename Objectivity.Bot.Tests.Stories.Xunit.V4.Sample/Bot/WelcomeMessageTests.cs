namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using global::Xunit;
    using StoryPerformer;

    public class WelcomeMessageTests: DemoBotTestBase
    {
        [Fact]
        public async Task ConversationInitiated_PlayStoryIsCalled_MustShowWelcomeMessage()
        {
            var story = this.Record
                .Bot.Says("Welcome to demo bot")
                .Rewind();

            await this.Play(story);
        }
    }
}
