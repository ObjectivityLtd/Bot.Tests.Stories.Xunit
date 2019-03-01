namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;
    using Configuration;

    public class Story<T> : IStory<T>
    {
        public IConfiguration Configuration { get; set; }

        public IList<IStoryFrame<T>> StoryFrames { get; } = new List<IStoryFrame<T>>();

        public void AddStoryFrame(IStoryFrame<T> storyFrame)
        {
            this.StoryFrames.Add(storyFrame);
        }
    }
}