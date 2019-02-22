namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using global::Xunit;
    using StoryPerformer;

    public class GreetingTests: DemoBotTestBase
    {
        [Fact]
        public async Task TwoFloorsPassed_PlayStoryIsCalled_MustShowMotivationMessage()
        {
            var story = this.Record
                .Bot.Says("Welcome to demo bot")
                .User.Says("hello")
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How many floors have you climbed today?")
                .User.Says("2")
                .Bot.Says("Come back when it's at least 3 John.")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task ThreeFloorsPassed_PlayStoryIsCalled_MustShowMotivationMessage()
        {
            var story = this.Record
                .Bot.Says("Welcome to demo bot")
                .User.Says("hello")
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How many floors have you climbed today?")
                .User.Says("3")
                .Bot.Says("Great score!")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task InvalidNumberPassed_PlayStoryIsCalled_MustRepromtForNumberOfFloors()
        {
            var story = this.Record
                .Bot.Says("Welcome to demo bot")
                .User.Says("hello")
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How many floors have you climbed today?")
                .User.Says("no")
                .Bot.Says("How many floors have you climbed today?")
                .Rewind();

            await this.Play(story);
        }
    }
}
