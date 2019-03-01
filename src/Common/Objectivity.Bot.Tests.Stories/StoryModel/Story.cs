namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;
    using Config;

    public class Story<T> : IStory<T>
    {
        public IConfig Config { get; set; }

        public IList<IStoryFrame<T>> StoryFrames { get; } = new List<IStoryFrame<T>>();

        public void AddStoryFrame(IStoryFrame<T> storyFrame)
        {
            this.StoryFrames.Add(storyFrame);
        }
    }
}