namespace Objectivity.Bot.Tests.Stories.Extensions
{
    using System.Linq;
    using StoryModel;

    public static class StoryExtensions
    {
        public static IStory<T> Concat<T>(this IStory<T> firstStory, IStory<T> secondStory)
        {
            var frames = secondStory.StoryFrames.Concat(firstStory.StoryFrames);
            var story = new Story<T>();

            foreach (var frame in frames)
            {
                story.AddStoryFrame(frame);
            }

            return story;
        }
    }
}
