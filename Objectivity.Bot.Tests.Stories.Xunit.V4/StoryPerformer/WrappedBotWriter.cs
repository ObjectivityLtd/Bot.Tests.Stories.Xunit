namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core;
    using Exceptions;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Adapters;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using StoryModel;
    using Xunit.StoryPerformer.IO;

    public class WrappedBotWriter : IDialogWriter<IMessageActivity>
    {
        private readonly IServiceProvider scopeContext;
        private readonly IConversationService conversationService;

        public WrappedBotWriter(IServiceProvider scopeContext, IConversationService conversationService)
        {
            this.scopeContext = scopeContext;
            this.conversationService = conversationService;
        }

        public async Task SendActivity(IMessageActivity messageActivity)
        {
            try
            {
                var bot = this.scopeContext.GetService<IBot>();
                var adapter = new TestAdapter();
                var context = new TurnContext(adapter, messageActivity as Activity);
                var queue = this.scopeContext.GetService<Queue<IMessageActivity>>();

                await bot.OnTurnAsync(context);

                while (adapter.ActiveQueue.Count > 0)
                {
                    queue.Enqueue(adapter.ActiveQueue.Dequeue());
                }
            }
            catch (MockException mockException)
            {
                var message = messageActivity?.Text;

                throw new UnmatchedUtteranceException(
                    $"Error while sending user message - matching intent couldn't be found. Check if unit test registers intent for the following utterance: {message}.",
                    mockException);
            }
        }

        public Task<IMessageActivity> GetStepMessageActivity(IStoryFrame<IMessageActivity> frame)
        {
            var activityBuilder = frame.ActivityBuilder ?? new MessageActivityBuilder(this.conversationService);

            var activity = activityBuilder.Build(frame);

            return Task.FromResult(activity);
        }
    }
}
