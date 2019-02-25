namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using Microsoft.Bot.Connector;
    using Stories.Core;

    public class ConversationService : IConversationService<IMessageActivity>
    {
        private readonly string userId = Guid.NewGuid().ToString();
        private readonly IMessageActivity toBotMessageActivity;

        public ConversationService(ChannelAccount from)
        {
            this.toBotMessageActivity = this.GetMessageActivity(from);
        }

        public string[] LatestOptions { get; set; }

        public IMessageActivity GetToBotActivity(string text)
        {
            this.toBotMessageActivity.Text = text;

            return this.toBotMessageActivity;
        }

        private IMessageActivity GetMessageActivity(ChannelAccount from)
        {
            var activity = DialogTestBase.MakeTestMessage();

            activity.From = from ?? new ChannelAccount { Id = this.userId };

            return activity;
        }
    }
}
