namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;

    public class Configuration<T> : IConfiguration, IConfigurationRecorder<T>
    {
        public Configuration(StoryRecorderBase<T> storyRecorder)
        {
            this.StoryRecorder = storyRecorder;
        }

        public string ChannelId { get; set; } = "Test";

        public ServiceCollection Services { get; } = new ServiceCollection();

        protected StoryRecorderBase<T> StoryRecorder { get; }

        public IStoryRecorder<T> RegisterService(Action<ServiceCollection> registration)
        {
            registration?.Invoke(this.Services);

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> UseChannel(string channelId)
        {
            this.ChannelId = channelId;
            return this.StoryRecorder;
        }
    }
}
