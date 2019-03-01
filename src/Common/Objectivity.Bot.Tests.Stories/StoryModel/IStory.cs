namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;
    using Config;

    public interface IStory<T>
    {
        IConfig Config { get; set; }

        IList<IStoryFrame<T>> StoryFrames { get; }

        void AddStoryFrame(IStoryFrame<T> storyFrame);
    }
}