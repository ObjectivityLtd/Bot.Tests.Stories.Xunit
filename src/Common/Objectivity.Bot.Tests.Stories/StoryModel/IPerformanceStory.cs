namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;

    public interface IPerformanceStory<T>
    {
        List<PerformanceStep<T>> Steps { get; set; }

        void EnqueueStartupSteps(List<T> messageActivities, Actor actor);

        void AddStep(T messageActivity, Actor actor);

        void AddSteps(List<T> messageActivities, Actor actor);

        void PushStartupSteps();
    }
}
