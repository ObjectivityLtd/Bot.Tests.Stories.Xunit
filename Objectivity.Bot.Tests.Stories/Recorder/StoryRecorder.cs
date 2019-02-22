namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using StoryModel;

    public class StoryRecorder : IStoryRecorder
    {
        public StoryRecorder()
        {
            this.Bot = new BotRecorder(this);
            this.User = new UserRecorder(this);
        }

        public IBotRecorder Bot { get; }

        public IUserRecorder User { get; }

        internal IStory Story { get; } = new Story();

        public static IStoryRecorder Record()
        {
            return new StoryRecorder();
        }

        public IStory Rewind()
        {
            return this.Story;
        }
    }
}