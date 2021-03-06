﻿namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using global::Xunit;
    using Microsoft.Bot.Connector;
    using Stories.Core;

    public class ChannelConfigurationTests : DemoBotTestBase
    {
        [Fact]
        public async Task ConversationInitiatedWithNoChannel_PlayStoryIsCalled_MustShowDefaultWelcomeMessage()
        {
            var story = this.Record
                .Configuration.WithConversationUpdateMember(ChannelId.User)
                .Bot.Says("Welcome to demo bot")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task ConversationInitiatedWithFacebookChannel_PlayStoryIsCalled_MustShowFacebookWelcomeMessage()
        {
            var story = this.Record
                .Configuration.UseChannel(Channels.Facebook)
                .Configuration.WithConversationUpdateMember(ChannelId.User)
                .Bot.Says("Welcome to demo bot facebook")
                .Rewind();

            await this.Play(story);
        }
    }
}
