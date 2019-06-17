namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Globalization;
    using System.Threading.Tasks;
    using global::Xunit;

    public class GreetingTests : DemoBotTestBase
    {
        [Theory]
        [InlineData(17, "I'm sorry John but you must be at least 18 years old.")]
        [InlineData(18, "Thank you.")]
        [InlineData(20, "Thank you.")]
        public async Task GivenAge_PlayStoryIsCalled_MustReplyExpectedMessage(
            int age,
            string expectedBotReply)
        {
            var story = this.Record
                .User.Says("hello")
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How old are you?")
                .User.Says(age.ToString(CultureInfo.InvariantCulture))
                .Bot.Says(expectedBotReply)
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task InvalidNumberPassed_PlayStoryIsCalled_MustRepromptForAge()
        {
            var story = this.Record
                .User.Says("hello")
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How old are you?")
                .User.Says("NaN")
                .Bot.Says("Your answer must be a number. Please try again.")
                .Rewind();

            await this.Play(story);
        }
    }
}
