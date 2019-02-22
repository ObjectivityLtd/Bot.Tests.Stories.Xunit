namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;

    public interface IStory
    {
        IList<IStoryFrame> StoryFrames { get; }

        void AddStoryFrame(IStoryFrame storyFrame);
    }
}