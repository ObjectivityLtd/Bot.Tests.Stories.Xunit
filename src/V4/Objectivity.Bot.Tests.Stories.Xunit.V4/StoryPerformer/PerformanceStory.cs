namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using Microsoft.Bot.Schema;
    using StoryModel;

    public class PerformanceStory : PerformanceStory<IMessageActivity>
    {
        protected override PerformanceStep<IMessageActivity> GetPerformanceStep(IMessageActivity message, Actor actor)
        {
            var performanceStep = new PerformanceStep(message)
            {
                Actor = actor
            };

            return performanceStep;
        }
    }
}
