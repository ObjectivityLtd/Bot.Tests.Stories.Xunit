﻿namespace Objectivity.Bot.Tests.Stories.Xunit.V4
{
    using System.Threading.Tasks;
    using Core;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;
    using StoryModel;
    using StoryPerformer;

    /// <summary>
    /// Base class for dialog test.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type.</typeparam>
    public abstract class DialogTestBase<TDialog>
        where TDialog : Dialog
    {

        private readonly TestPlayer testPlayer;

        protected DialogTestBase()
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
        protected IStoryRecorder<IMessageActivity> Record => DialogStoryRecorder.Record();

        /// <summary>
        /// Gets data store for test session.
        /// </summary>
        protected IStorage DataStore { get; } = new MemoryStorage();

        /// <summary>
        /// Executes test scenario.
        /// </summary>
        /// <param name="story">Test story.</param>
        /// <returns>Awaitable task.</returns>
        protected async Task Play(IStory<IMessageActivity> story)
        {
            var services = new ServiceCollection();

            this.RegisterDummyBot(services);

            services.AddScoped<Dialog, TDialog>();

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

        /// <summary>
        /// Registers bot instance for test run.
        /// </summary>
        /// <param name="services">Services collection.</param>
        private void RegisterDummyBot(IServiceCollection services)
        {
            services.AddScoped<IBot, DummyDialogBot>();
            services.AddScoped(sp => new ConversationState(this.DataStore));
            services.AddScoped(sp => new UserState(this.DataStore));
        }
    }
}