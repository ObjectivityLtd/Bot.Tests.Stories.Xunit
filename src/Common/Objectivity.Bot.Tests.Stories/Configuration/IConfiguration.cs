namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public interface IConfiguration
    {
        IReadOnlyCollection<string> ConversationUpdateAddedMembers { get; }

        string ConversationId { get; set; }

        string ChannelId { get; set; }

        List<Action<ServiceCollection>> Registrations { get; }
    }
}
