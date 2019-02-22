namespace Objectivity.Bot.Tests.Stories.Xunit.V4
{
    using Core;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;
    using StoryPerformer;

    /// <summary>
    /// Base class for dialog test.
    /// </summary>
    /// <typeparam name="TDialog">Dialog type.</typeparam>
    public abstract class DialogTestBase<TDialog> : TestBase
        where TDialog : Dialog
    {
        protected IStoryRecorder<IMessageActivity> Record => DialogStoryRecorder.Record();

        /// <summary>
        /// Gets data store for test session.
        /// </summary>
        protected IStorage DataStore { get; } = new MemoryStorage();

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddScoped<Dialog, TDialog>();

            this.RegisterBot(services);
        }

        /// <summary>
        /// Registers bot instance for test run.
        /// </summary>
        /// <param name="services">Services collection.</param>
        protected virtual void RegisterBot(IServiceCollection services)
        {
            services.AddScoped<IBot, DummyDialogBot>();
            services.AddScoped(sp => new ConversationState(this.DataStore));
            services.AddScoped<UserState>(sp => new UserState(this.DataStore));
        }
    }
}
