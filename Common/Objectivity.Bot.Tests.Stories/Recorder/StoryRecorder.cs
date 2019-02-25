namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using StoryModel;

    public abstract class StoryRecorderBase<T> : IStoryRecorder<T>
    {
        public StoryRecorderBase()
        {
            this.Bot = new BotRecorder<T>(this);
            this.User = new UserRecorder<T>(this);
        }

        public IBotRecorder<T> Bot { get; }

        public IUserRecorder<T> User { get; }

        public IStory<T> Story { get; } = new Story<T>();

        public IStory<T> Rewind()
        {
            return this.Story;
        }
    }
}