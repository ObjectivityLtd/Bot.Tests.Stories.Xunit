namespace Objectivity.Bot.Tests.Stories.Xunit.V4
{
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;
    using StoryModel;
    using StoryPerformer;

    /// <summary>
    /// Base class for bot test.
    /// </summary>
    /// <typeparam name="TBot">Bot type.</typeparam>
    public abstract class BotTestBase<TBot>
        where TBot : class, IBot
    {
        private readonly TestPlayer testPlayer;

        protected BotTestBase()
        {
            this.testPlayer = new TestPlayer(this.From);
        }

        /// <summary>
        /// Gets or sets from channel account.
        /// </summary>
        protected ChannelAccount From { get; set; }

        /// <summary>
        /// Gets test story recorder.
        /// </summary>
        protected IStoryRecorder<IMessageActivity> Record => BotStoryRecorder.Record();

        /// <summary>
        /// Executes test scenario.
        /// </summary>
        /// <param name="story">Test story.</param>
        /// <returns>Awaitable task.</returns>
        protected async Task Play(IStory<IMessageActivity> story)
        {
            var services = new ServiceCollection();

            services.AddScoped<IBot, TBot>();
            services.AddScoped(sp => story.Configuration);

            this.testPlayer.ConfigureServices(services);
            this.ConfigureServices(services);

            await this.testPlayer.Play(story, this.From, services);
        }

        /// <summary>
        /// Configures test services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
