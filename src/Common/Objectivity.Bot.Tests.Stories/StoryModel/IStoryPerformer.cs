namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using StoryModel;

    public interface IStoryPerformer<T>
    {
        Task<List<PerformanceStep<T>>> Perform(IStory<T> testStory);
    }
}
