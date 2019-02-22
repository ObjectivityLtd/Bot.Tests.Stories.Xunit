namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using Microsoft.Bot.Schema;

    public class ConversationService : IConversationService
    {
        private readonly IMessageActivity toBotMessageActivity = new Activity();

        public string[] LatestOptions { get; set; }

        public ConversationAccount Account { get; } = new ConversationAccount { Id = Guid.NewGuid().ToString() };

        public IMessageActivity GetToBotActivity(string text)
        {
            this.toBotMessageActivity.Text = text;

            return this.toBotMessageActivity;
        }
    }
}
