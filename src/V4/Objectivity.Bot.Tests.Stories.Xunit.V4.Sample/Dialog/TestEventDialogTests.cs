namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System;
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using DemoBot.Dialogs.Prompts;
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
                .Bot.SendsEvent(TestAuthPrompt.AuthRequestEventName)
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
                .Bot.SendsEvent(TestAuthPrompt.AuthRequestEventName)
                .User.SendsActivity(new Activity
                {
                    Type = ActivityTypes.Event,
                    Name = TestAuthPrompt.AuthResponseEventName,
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
                .Bot.SendsEvent(TestAuthPrompt.AuthRequestEventName)
                .User.Says("Show info")
                .Bot.Says("You're unauthorized")
                .Rewind();

            await this.Play(story);
        }
    }
}
