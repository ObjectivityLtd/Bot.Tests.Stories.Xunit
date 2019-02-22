namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;

    public interface IStory<T>
    {
        IList<IStoryFrame<T>> StoryFrames { get; }

        void AddStoryFrame(IStoryFrame<T> storyFrame);
    }
}