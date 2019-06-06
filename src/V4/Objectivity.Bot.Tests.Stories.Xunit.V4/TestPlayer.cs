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

    internal class TestPlayer
    {
        public TestPlayer(ChannelAccount from)
        {
            this.From = from;
        }

        protected ChannelAccount From { get; set; }

        public async Task Play(IStory<IMessageActivity> story, ChannelAccount from, IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var player = new StoryPlayer(provider);

            await player.Play(story);
        }

        public void ConfigureServices(IServiceCollection services, IStory<IMessageActivity> story)
        {
            services.AddScoped<StoryAsserts>();
            services.AddScoped(x => new Queue<IMessageActivity>());
            services.AddScoped<IStoryPerformer<IMessageActivity>, WrappedStoryPerformer>();
            services.AddScoped<IConversationService>(x => new ConversationService(story.Configuration));
            services.AddScoped(x => this.From);
            services.AddScoped<WrappedDialogResult>();
            services.AddScoped<FinishStepAsserts>();

            services.AddScoped<IDialogWriter<IMessageActivity>, WrappedBotWriter>();
            services.AddScoped<IDialogReader<IMessageActivity>, WrappedBotReader>();
        }
    }
}