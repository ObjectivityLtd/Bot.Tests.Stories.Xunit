namespace Objectivity.Bot.Tests.Stories.Config
{
    using Microsoft.Extensions.DependencyInjection;

    public interface IConfig
    {
        string ChannelId { get; set; }

        ServiceCollection Services { get; }
    }
}
