namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public interface IConfiguration
    {
        string ChannelId { get; set; }

        ServiceCollection Services { get; }

        List<Action<ServiceCollection>> Registrations { get; }
    }
}
