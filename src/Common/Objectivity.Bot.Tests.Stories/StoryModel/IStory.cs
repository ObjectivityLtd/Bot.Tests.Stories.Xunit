namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;
    using Configuration;

    public interface IStory<T>
    {
        IConfiguration Configuration { get; set; }

        IList<IStoryFrame<T>> StoryFrames { get; }

        void AddStoryFrame(IStoryFrame<T> storyFrame);
    }
}