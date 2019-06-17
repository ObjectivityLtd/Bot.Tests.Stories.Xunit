namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using global::Xunit;
    using global::Xunit.Sdk;
    using Stories.Core;

    public class UserInitiatedDialogTests : DemoBotTestBase
    {
        [Fact]
        public async Task FullStory_PlayStoryIsCalled_DialogFlowIsCorrect()
        {
            var story = this.Record
                .Configuration.WithConversationUpdateMember(ChannelId.User)
                .Bot.Says("Welcome to demo bot")
                .User.Says("hi")
                .Bot.Says("You said: hi")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task StoryWithWrongInitialActor_PlayStoryIsCalled_EqualExceptionThrown()
        {
            var story = this.Record
                .Configuration.WithConversationUpdateMember(ChannelId.User)
                .Bot.Says("Welcome to demo bot")
                .Bot.Says("hi")
                .User.Says("hi")
                .Rewind();

            await Assert.ThrowsAsync<TrueException>(async () =>
            {
                await this.Play(story);
            });
        }
    }
}
