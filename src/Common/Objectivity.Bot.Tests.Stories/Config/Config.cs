namespace Objectivity.Bot.Tests.Stories.Config
{
    using Recorder;

    public class Config<T> : IConfig<T>
    {
        public Config(StoryRecorderBase<T> storyRecorder)
        {
            this.StoryRecorder = storyRecorder;
        }

        protected StoryRecorderBase<T> StoryRecorder { get; }

        private string ChannelId { get; set; } = "Test";

        public IStoryRecorder<T> UseChannel(string channelId)
        {
            this.ChannelId = channelId;
            return this.StoryRecorder;
        }
    }
}
