namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer.IO
{
    using System.Collections.Generic;
    using System.Linq;
    using Autofac;
    using Core;
    using Microsoft.Bot.Connector;

    public class WrapperDialogReader : IDialogReader<IMessageActivity>
    {
        private readonly IScopeContext scopeContext;
        private readonly Queue<IMessageActivity> startupActivities = new Queue<IMessageActivity>();

        public WrapperDialogReader(IScopeContext scopeContext)
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
            var queue = this.scopeContext.Scope.Resolve<Queue<IMessageActivity>>();
            var enqueueToStartupActivities = false;
            var result = new List<IMessageActivity>();

            while (queue.Any())
            {
                var messageActivity = queue.Dequeue();

                if (messageActivity.Text == Constants.WrapperStartMessage)
                {
                    enqueueToStartupActivities = true;
                }

                if (enqueueToStartupActivities)
                {
                    this.startupActivities.Enqueue(messageActivity);
                }
                else
                {
                    result.Add(messageActivity);
                }
            }

            return result;
        }
    }
}
