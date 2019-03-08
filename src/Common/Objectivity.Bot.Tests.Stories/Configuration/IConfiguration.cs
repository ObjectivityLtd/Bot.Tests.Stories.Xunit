namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public interface IConfiguration
    {
        string ChannelId { get; set; }

        List<Action<ServiceCollection>> Registrations { get; }
    }
}
