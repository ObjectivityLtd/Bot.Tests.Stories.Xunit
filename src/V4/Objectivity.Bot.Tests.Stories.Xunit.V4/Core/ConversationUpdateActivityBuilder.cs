namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using System.Linq;
    using Microsoft.Bot.Schema;
    using Stories.Configuration;
    using Stories.Core;
    using StoryModel;

    public class ConversationUpdateActivityBuilder : IActivityBuilder<IMessageActivity>
    {
        private readonly IConfiguration configuration;

        public ConversationUpdateActivityBuilder(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IMessageActivity Build(IStoryFrame<IMessageActivity> frame)
        {
            return new Activity
            {
                Id = Guid.NewGuid().ToString(),
                Type = ActivityTypes.ConversationUpdate,
                From = new ChannelAccount { Id = ChannelId.User },
                Conversation = new ConversationAccount { Id = Guid.NewGuid().ToString() },
                Recipient = new ChannelAccount { Id = ChannelId.Bot },
                ServiceUrl = "InvalidServiceUrl",
                ChannelId = this.configuration.ChannelId,
                Attachments = Array.Empty<Attachment>(),
                Entities = Array.Empty<Entity>(),
                MembersAdded = this.configuration.ConversationUpdateAddedMembers?
                    .Select(x => new ChannelAccount(x))
                    .ToList(),
            };
        }
    }
}