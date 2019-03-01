namespace Objectivity.Bot.Tests.Stories.Config
{
    using Recorder;

    public class Config<T> : IConfig, IConfigRecorder<T>
    {
        public Config(StoryRecorderBase<T> storyRecorder)
        {
            this.StoryRecorder = storyRecorder;
        }

        public string ChannelId { get; set; } = "Test";

        protected StoryRecorderBase<T> StoryRecorder { get; }

        public IStoryRecorder<T> UseChannel(string channelId)
        {
            this.ChannelId = channelId;
            return this.StoryRecorder;
        }
    }
}
