namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using Microsoft.Bot.Connector;
    using Stories.Core;
    using Stories.Exceptions;
    using StoryModel;

    public class MessageActivityBuilder : IActivityBuilder<IMessageActivity>
    {
        private readonly IConversationService<IMessageActivity> conversationService;

        public MessageActivityBuilder(IConversationService<IMessageActivity> conversationService)
        {
            this.conversationService = conversationService;
        }

        public IMessageActivity Build(IStoryFrame<IMessageActivity> frame)
        {
            var text = this.GetUserStepMessage(frame);

            return new Activity()
            {
                Id = Guid.NewGuid().ToString(),
                Type = ActivityTypes.Message,
                From = new ChannelAccount { Id = ChannelID.User },
                Text = text,
                Conversation = new ConversationAccount { Id = Guid.NewGuid().ToString() },
                Recipient = new ChannelAccount { Id = ChannelID.Bot },
                ServiceUrl = "InvalidServiceUrl",
                ChannelId = "Test",
                Attachments = Array.Empty<Attachment>(),
                Entities = Array.Empty<Entity>(),
            };
        }

        private string GetUserStepMessage(IStoryFrame<IMessageActivity> frame)
        {
            switch (frame.ComparisonType)
            {
                case ComparisonType.Option:
                    return this.GetUserOptionMessage(frame);
                default:
                    return frame.Text;
            }
        }

        private string GetUserOptionMessage(IStoryFrame<IMessageActivity> frame)
        {
            if (this.conversationService.LatestOptions == null)
            {
                throw new MissingOptionsException();
            }

            var index = frame.OptionIndex;

            if (index < 0 || this.conversationService.LatestOptions.Length <= index)
            {
                throw new OptionsIndexOutOfRangeException(index, this.conversationService.LatestOptions.Length);
            }

            return this.conversationService.LatestOptions[index];
        }
    }
}
