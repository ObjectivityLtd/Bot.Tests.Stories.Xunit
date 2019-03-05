namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using DemoBot.Services;
    using global::Xunit;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    public class ServiceMockTests : DemoBotTestBase
    {
        [Fact]
        public async Task ConversationInitiatedWithServiceMock_PlayStoryIsCalled_MustReturnMockedResults()
        {
            var expectedResult = 0;
            var roomServiceMock = new Mock<IRoomService>();
            roomServiceMock.Setup(x => x.GetRoomFloorByNumber(It.IsAny<decimal>())).Returns(expectedResult);

            var story = this.Record
                .Config.RegisterService(services => services.AddScoped(sp => roomServiceMock.Object))
                .Bot.Says("Welcome to demo bot")
                .User.Says("room test")
                .Bot.Says("What's the room number?")
                .User.Says("1.55")
                .Bot.Says($"Room floor is: {expectedResult}")
                .Rewind();

            await this.Play(story);
        }
    }
}
