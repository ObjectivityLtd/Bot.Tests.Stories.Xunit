namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;

    public class Story : IStory
    {
        public IList<IStoryFrame> StoryFrames { get; } = new List<IStoryFrame>();

        public void AddStoryFrame(IStoryFrame storyFrame)
        {
            this.StoryFrames.Add(storyFrame);
        }
    }
}