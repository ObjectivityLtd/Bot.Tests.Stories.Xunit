namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading;
    using System.Threading.Tasks;
    using global::Xunit;
    using Microsoft.Bot.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;

    public class MiddlewareTests : DemoBotTestBase
    {
        [Fact]
        public async Task MiddlewareRegistered_PlayStoryIsCalled_MiddlewareIsExecuted()
        {
            var fakeMiddleware = new Mock<IMiddleware>();

            var story = this.Record
                .Configuration.RegisterService(
                    services => services.AddScoped(factory => fakeMiddleware.Object))
                .Rewind();

            await this.Play(story);

            fakeMiddleware.Verify(
                x => x.OnTurnAsync(
                    It.IsAny<ITurnContext>(),
                    It.IsAny<NextDelegate>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
