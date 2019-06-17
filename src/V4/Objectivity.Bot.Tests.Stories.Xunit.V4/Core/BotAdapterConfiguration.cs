namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System.Collections.Generic;

    public class BotAdapterConfiguration
    {
        public IList<UserAccessTokenConfiguration> UserAccessTokens { get; }
            = new List<UserAccessTokenConfiguration>();
    }
}
