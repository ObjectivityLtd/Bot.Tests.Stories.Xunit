namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStoryPerformer<T>
    {
        Task<List<PerformanceStep<T>>> Perform(IStory<T> testStory);
    }
}
