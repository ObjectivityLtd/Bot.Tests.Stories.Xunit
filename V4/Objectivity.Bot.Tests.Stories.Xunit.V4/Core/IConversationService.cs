namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using Microsoft.Bot.Schema;
    using Stories.Core;

    public interface IConversationService : IConversationService<IMessageActivity>
    {
        ConversationAccount Account { get; }
    }
}
