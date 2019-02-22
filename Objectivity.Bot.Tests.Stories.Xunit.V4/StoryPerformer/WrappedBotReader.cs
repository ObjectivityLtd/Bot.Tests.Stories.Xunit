namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit.StoryPerformer.IO;

    public class WrappedBotReader : IDialogReader<IMessageActivity>
    {
        private readonly IServiceProvider scopeContext;
        private readonly Queue<IMessageActivity> startupActivities = new Queue<IMessageActivity>();

        public WrappedBotReader(IServiceProvider scopeContext)
        {
            this.scopeContext = scopeContext;
        }

        public List<IMessageActivity> DequeueStartupMessageActivities()
        {
            var result = new List<IMessageActivity>();

            while (this.startupActivities.Any())
            {
                var activity = this.startupActivities.Dequeue();
                result.Add(activity);
            }

            return result;
        }

        public List<IMessageActivity> GetMessageActivities()
        {
            var queue = this.scopeContext.GetService<Queue<IMessageActivity>>();
            var result = new List<IMessageActivity>();

            while (queue.Any())
            {
                var messageActivity = queue.Dequeue();

                result.Add(messageActivity);
            }

            return result;
        }
    }
}
