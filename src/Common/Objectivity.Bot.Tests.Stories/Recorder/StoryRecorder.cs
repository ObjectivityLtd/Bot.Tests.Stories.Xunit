namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using Configuration;
    using StoryModel;

    public abstract class StoryRecorderBase<T> : IStoryRecorder<T>
    {
        protected StoryRecorderBase()
        {
            this.Bot = new BotRecorder<T>(this);
            this.User = new UserRecorder<T>(this);
            this.Configuration = new Configuration<T>(this);
            this.Story = new Story<T>();
        }

        public IBotRecorder<T> Bot { get; }

        public IUserRecorder<T> User { get; }

        public Configuration<T> Configuration { get; }

        public IStory<T> Story { get; }

        public IStory<T> Rewind()
        {
            this.Story.Configuration = this.Configuration;
            return this.Story;
        }
    }
}