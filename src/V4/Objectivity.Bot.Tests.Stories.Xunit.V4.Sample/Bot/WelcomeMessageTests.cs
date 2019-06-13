namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using Extensions;
    using global::Xunit;
    using Stories.Core;

    public class WelcomeMessageTests : DemoBotTestBase
    {
        [Fact]
        public async Task BotJoinedConversation_PlayStoryIsCalled_MustNotShowWelcomeMessage()
        {
            var story = this.Record
                .Configuration.SetConversationUpdateMembers(ChannelId.Bot)
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task UserJoinedConversation_PlayStoryIsCalled_MustShowWelcomeMessage()
        {
            var story = this.Record
                .Configuration.SetConversationUpdateMembers(ChannelId.User)
                .Bot.Says("Welcome to demo bot")
                .Rewind();

            await this.Play(story);
        }
    }
}
