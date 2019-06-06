namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System;
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using global::Xunit;
    using Microsoft.Bot.Schema;
    using Objectivity.Bot.Tests.Stories.Core;
    using Objectivity.Bot.Tests.Stories.Xunit.V4.Extensions;

    public class TestEventDialogTests : DialogTestBase<TestEventDialog>
    {
        [Fact]
        public async Task GivenDialogStarts_PlayStoryIsCalled_MustReturnAuthorizationRequestEvent()
        {
            var story = this.Record
                .Bot.SendsEvent("tokens/request")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task GivenAuthenticationResponseSent_PlayStoryIsCalled_MustReturnAuthorizationConfirmation()
        {
            const string channelId = "TestChannel";
            var conversationId = Guid.NewGuid().ToString();

            var story = this.Record
                .Configuration.UseChannel(channelId)
                .Configuration.WithConversationId(conversationId)
                .Bot.SendsEvent("tokens/request")
                .User.SendsActivity(new Activity
                {
                    Type = ActivityTypes.Event,
                    Name = "tokens/response",
                    ChannelId = channelId,
                    Conversation = new ConversationAccount { Id = conversationId },
                    From = new ChannelAccount { Id = ChannelId.User },
                    Recipient = new ChannelAccount { Id = ChannelId.Bot },
                })
                .Bot.Says("You're authorized")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task GivenAuthenticationResponseWasNotSent_PlayStoryIsCalled_MustReturnAuthorizationDeniedMessage()
        {
            var story = this.Record
                .Bot.SendsEvent("tokens/request")
                .User.Says("Show info")
                .Bot.Says("You're unauthorized")
                .Rewind();

            await this.Play(story);
        }
    }
}
