namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Bot.Schema;
    using Stories.Core;
    using StoryModel;

    public class ConversationUpdateActivityBuilder : IActivityBuilder<IMessageActivity>
    {
        public IMessageActivity Build(IStoryFrame<IMessageActivity> frame)
        {
            return new Activity()
            {
                Id = Guid.NewGuid().ToString(),
                Type = ActivityTypes.ConversationUpdate,
                From = new ChannelAccount { Id = ChannelID.User },
                Conversation = new ConversationAccount { Id = Guid.NewGuid().ToString() },
                Recipient = new ChannelAccount { Id = ChannelID.Bot },
                ServiceUrl = "InvalidServiceUrl",
                ChannelId = "Test",
                Attachments = Array.Empty<Attachment>(),
                Entities = Array.Empty<Entity>(),
                MembersAdded = new List<ChannelAccount>
                {
                    new ChannelAccount(ChannelID.User),
                },
            };
        }
    }
}