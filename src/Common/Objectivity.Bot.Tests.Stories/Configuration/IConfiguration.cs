namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IConfiguration
    {
        string ChannelId { get; set; }

        ServiceCollection Services { get; }
    }
}
