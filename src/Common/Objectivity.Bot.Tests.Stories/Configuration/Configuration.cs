namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;

    public class Configuration<T> : IConfiguration, IConfigurationRecorder<T>
    {
        private readonly List<string> conversationUpdateMembers = new List<string>();

        public Configuration(StoryRecorderBase<T> storyRecorder)
        {
            this.StoryRecorder = storyRecorder;
        }

        public string ChannelId { get; set; } = "Test";

        public IReadOnlyCollection<string> ConversationUpdateAddedMembers => this.conversationUpdateMembers.AsReadOnly();

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

        public IStoryRecorder<T> WithConversationUpdateMember(string userId)
        {
            this.conversationUpdateMembers.Add(userId);

            return this.StoryRecorder;
        }
    }
}
