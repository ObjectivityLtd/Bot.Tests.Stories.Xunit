namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Microsoft.Bot.Connector;
    using StoryModel;

    public class PerformanceStory : IPerformanceStory
    {
        private const string TypingMessageType = "typing";

        private readonly Queue<PerformanceStep> startupSteps = new Queue<PerformanceStep>();

        public PerformanceStory()
        {
            this.Steps = new List<PerformanceStep>();
        }

        public List<PerformanceStep> Steps { get; set; }

        public void EnqueueStartupStep(IMessageActivity messageActivity, Actor actor)
        {
            var step = GetPerformanceStep(messageActivity, actor);

            if (step != null)
            {
                this.startupSteps.Enqueue(step);
            }
        }

        public void EnqueueStartupSteps(List<IMessageActivity> messageActivities, Actor actor)
        {
            foreach (var messageActivity in messageActivities)
            {
                this.EnqueueStartupStep(messageActivity, actor);
            }
        }

        public void AddStep(IMessageActivity messageActivity, Actor actor)
        {
            var step = GetPerformanceStep(messageActivity, actor);

            this.Steps.AddNotNullStep(step);
        }

        public void AddSteps(List<IMessageActivity> messageActivities, Actor actor)
        {
            foreach (var messageActivity in messageActivities)
            {
                this.AddStep(messageActivity, actor);
            }
        }

        public void PushStartupSteps()
        {
            while (this.startupSteps.Any())
            {
                var step = this.startupSteps.Dequeue();
                this.Steps.AddNotNullStep(step);
            }
        }

        private static PerformanceStep GetPerformanceStep(IMessageActivity message, Actor actor)
        {
            if (message.Type == TypingMessageType || message.Text == Consts.WrapperStartMessage)
            {
                return null;
            }

            var performanceStep = new PerformanceStep(message)
            {
                Actor = actor
            };

            return performanceStep;
        }
    }
}
