namespace Objectivity.Bot.Tests.Stories.Player
{
    using StoryModel;

    public class StoryPlayerStep<T>
    {
        public IStoryFrame<T> StoryFrame { get; set; }

        public StoryPlayerStepStatus Status { get; set; }
    }
}
