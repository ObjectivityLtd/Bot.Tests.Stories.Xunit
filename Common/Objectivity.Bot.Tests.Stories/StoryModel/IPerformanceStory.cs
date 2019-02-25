namespace Objectivity.Bot.Tests.Stories.StoryPerformer
{
    using System.Collections.Generic;
    using StoryModel;

    public interface IPerformanceStory<T>
    {
        List<PerformanceStep<T>> Steps { get; set; }

        void EnqueueStartupStep(T messageActivity, Actor actor);

        void EnqueueStartupSteps(List<T> messageActivities, Actor actor);

        void AddStep(T messageActivity, Actor actor);

        void AddSteps(List<T> messageActivities, Actor actor);

        void PushStartupSteps();
    }
}
