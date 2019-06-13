namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System.Collections.Generic;

    public class BotAdapterConfiguration
    {
        public IList<(string ConnectionName, string ChannelId, string UserId, string Token, string MagicCode)> UserAccessTokens { get; }
            = new List<(string ConnectionName, string ChannelId, string UserId, string Token, string MagicCode)>();
    }
}
