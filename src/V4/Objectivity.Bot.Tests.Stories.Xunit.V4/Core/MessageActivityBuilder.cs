namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using Configuration;
    using Exceptions;
    using Microsoft.Bot.Schema;
    using Stories.Core;
    using StoryModel;

    public class MessageActivityBuilder : IActivityBuilder<IMessageActivity>
    {
        private readonly IConversationService conversationService;
        private readonly IConfiguration config;

        public MessageActivityBuilder(
            IConversationService conversationService,
            IConfiguration config)
        {
            this.conversationService = conversationService;
            this.config = config;
        }

        public IMessageActivity Build(IStoryFrame<IMessageActivity> frame)
        {
            var text = this.GetUserStepMessage(frame);

            return new Activity()
            {
                Id = Guid.NewGuid().ToString(),
                Type = ActivityTypes.Message,
                From = new ChannelAccount { Id = ChannelId.User },
                Text = text,
                Conversation = this.conversationService.Account,
                Recipient = new ChannelAccount { Id = ChannelId.Bot },
                ServiceUrl = "InvalidServiceUrl",
                ChannelId = this.config.ChannelId,
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
