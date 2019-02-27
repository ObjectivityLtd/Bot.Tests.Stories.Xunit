namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public abstract class PerformanceStory<T> : IPerformanceStory<T>
    {
        private readonly Queue<PerformanceStep<T>> startupSteps = new Queue<PerformanceStep<T>>();

        protected PerformanceStory()
        {
            this.Steps = new List<PerformanceStep<T>>();
        }

        public List<PerformanceStep<T>> Steps { get; set; }

        public void EnqueueStartupSteps(List<T> messageActivities, Actor actor)
        {
            foreach (var messageActivity in messageActivities)
            {
                this.EnqueueStartupStep(messageActivity, actor);
            }
        }

        public void AddStep(T messageActivity, Actor actor)
        {
            var step = this.GetPerformanceStep(messageActivity, actor);

            this.Steps.AddNotNullStep(step);
        }

        public void AddSteps(List<T> messageActivities, Actor actor)
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

        protected abstract PerformanceStep<T> GetPerformanceStep(T message, Actor actor);

        private void EnqueueStartupStep(T messageActivity, Actor actor)
        {
            var step = this.GetPerformanceStep(messageActivity, actor);

            if (step != null)
            {
                this.startupSteps.Enqueue(step);
            }
        }
    }
}
