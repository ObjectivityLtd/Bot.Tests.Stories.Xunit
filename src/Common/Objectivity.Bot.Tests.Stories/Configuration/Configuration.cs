namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;

    public class Configuration<T> : IConfiguration, IConfigurationRecorder<T>
    {
        public Configuration(StoryRecorderBase<T> storyRecorder)
        {
            this.StoryRecorder = storyRecorder;
        }

        public string ChannelId { get; set; } = "Test";

        public string[] ConversationUpdateAddedMembers { get; set; } = new[]
        {
            Core.ChannelId.User,
        };

        public string ConversationId { get; set; }

        public List<Action<ServiceCollection>> Registrations { get; } = new List<Action<ServiceCollection>>();

        protected StoryRecorderBase<T> StoryRecorder { get; }

        public IStoryRecorder<T> RegisterService(Action<ServiceCollection> registration)
        {
            this.Registrations.Add(registration);
            return this.StoryRecorder;
        }

        public IStoryRecorder<T> UseChannel(string channelId)
        {
            this.ChannelId = channelId;
            return this.StoryRecorder;
        }

        public IStoryRecorder<T> WithConversationId(string conversationId)
        {
            this.ConversationId = conversationId;
            return this.StoryRecorder;
        }

        public IStoryRecorder<T> SetConversationUpdateMembers(params string[] userIds)
        {
            this.ConversationUpdateAddedMembers = userIds;

            return this.StoryRecorder;
        }
    }
}
