namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using Recorder;

    public class Configuration<T> : IConfiguration, IConfigurationRecorder<T>
    {
        public Configuration(StoryRecorderBase<T> storyRecorder)
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
