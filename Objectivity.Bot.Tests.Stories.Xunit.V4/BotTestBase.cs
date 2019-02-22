namespace Objectivity.Bot.Tests.Stories.Xunit.V4
{
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;
    using StoryPerformer;

    /// <summary>
    /// Base class for bot test.
    /// </summary>
    /// <typeparam name="TBot">Bot type.</typeparam>
    public abstract class BotTestBase<TBot> : TestBase
        where TBot : class, IBot
    {
        protected IStoryRecorder<IMessageActivity> Record => BotStoryRecorder.Record();

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddScoped<IBot, TBot>();
        }
    }
}
