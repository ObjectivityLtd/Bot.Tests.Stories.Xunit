namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using Microsoft.Bot.Schema;
    using Objectivity.Bot.Tests.Stories.Configuration;

    public class ConversationService : IConversationService
    {
        private readonly IMessageActivity toBotMessageActivity = new Activity();

        public ConversationService(IConfiguration configuration)
        {
            this.Account = new ConversationAccount
            {
                Id = configuration?.ConversationId ?? Guid.NewGuid().ToString(),
            };
        }

        public string[] LatestOptions { get; set; }

        public ConversationAccount Account { get; }

        public IMessageActivity GetToBotActivity(string text)
        {
            this.toBotMessageActivity.Text = text;

            return this.toBotMessageActivity;
        }
    }
}
