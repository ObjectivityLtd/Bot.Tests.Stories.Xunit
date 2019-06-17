namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using Core;
    using global::Xunit;
    using Stories.Core;

    public class UserAccessTokenTests : DemoBotTestBase
    {
        [Fact]
        public async Task UserDoesNotHaveToken_PlayStoryIsCalled_BotDidNotFindToken()
        {
            var story = this.Record
                .User.Says("token test")
                .Bot.Says($"No tokens found")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task UserHasToken_PlayStoryIsCalled_BotFoundToken()
        {
            const string channelId = "testChannel";
            const string connectionName = "testConnection";

            this.BotAdapterConfiguration.UserAccessTokens.Add(new UserAccessTokenConfiguration
            {
                UserId = ChannelId.User,
                ConnectionName = connectionName,
                ChannelId = channelId,
            });

            var story = this.Record
                .Configuration.UseChannel(channelId)
                .User.Says("token test")
                .Bot.Says($"Token for connection {connectionName} found")
                .Rewind();

            await this.Play(story);
        }
    }
}
