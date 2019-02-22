namespace Objectivity.Bot.Tests.Stories.Xunit.V4
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Asserts;
    using Core;
    using Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using StoryModel;
    using StoryPerformer;
    using Xunit.StoryPerformer;
    using Xunit.StoryPerformer.IO;

    public abstract class TestBase
    {
        /// <summary>
        /// Gets or sets from channel account.
        /// </summary>
        protected ChannelAccount From { get; set; }

        /// <summary>
        /// Executes test scenario.
        /// </summary>
        /// <param name="story">Test story.</param>
        /// <returns>Awaitable task.</returns>
        protected async Task Play(IStory<IMessageActivity> story)
        {
            var services = new ServiceCollection();

            this.ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            var player = new StoryPlayer(provider);

            await player.Play(story);
        }

        /// <summary>
        /// Configures test services.
        /// </summary>
        /// <param name="services">Service collection.</param>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<StoryAsserts>();
            services.AddScoped<Queue<IMessageActivity>>(x => new Queue<IMessageActivity>());
            services.AddScoped<IStoryPerformer<IMessageActivity>, WrappedStoryPerformer>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<ChannelAccount>(x => this.From);
            services.AddScoped<WrappedDialogResult>();
            services.AddScoped<FinishStepAsserts>();

            services.AddScoped<IDialogWriter<IMessageActivity>, WrappedBotWriter>();
            services.AddScoped<IDialogReader<IMessageActivity>, WrappedBotReader>();
        }
    }
}