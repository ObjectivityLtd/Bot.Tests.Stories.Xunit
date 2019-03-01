namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using global::Xunit;
    using Microsoft.Bot.Connector;

    public class ChannelConfigTests : DemoBotTestBase
    {
        [Fact]
        public async Task UseDefaultChannel()
        {
            var story = this.Record
                .Config.UseChannel("")
                .Bot.Says("Welcome to demo bot")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task UseFacebookChannel()
        {
            var story = this.Record
                .Config.UseChannel(Channels.Facebook)
                .Bot.Says("Welcome to demo bot")
                .Rewind();

            await this.Play(story);
        }
    }
}
