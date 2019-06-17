namespace Objectivity.Bot.Tests.Stories.Xunit.V4
{
    using Core;

    public class TestBase
    {
        /// <summary>
        /// Gets bot adapter configuration.
        /// </summary>
        public IBotAdapterConfiguration BotAdapterConfiguration { get; } = new BotAdapterConfiguration();
    }
}
