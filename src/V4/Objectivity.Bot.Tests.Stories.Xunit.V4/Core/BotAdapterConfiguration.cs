namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System;
    using System.Collections.Generic;

    public class BotAdapterConfiguration : IBotAdapterConfiguration
    {
        private readonly List<UserAccessTokenConfiguration> userAccessTokens = new List<UserAccessTokenConfiguration>();

        public IReadOnlyCollection<UserAccessTokenConfiguration> UserAccessTokens => this.userAccessTokens.AsReadOnly();

        public void WithUserAccessTokenConfiguration(UserAccessTokenConfiguration tokenConfig)
        {
            if (tokenConfig == null)
            {
                throw new ArgumentNullException(nameof(tokenConfig));
            }

            this.userAccessTokens.Add(tokenConfig);
        }
    }
}
