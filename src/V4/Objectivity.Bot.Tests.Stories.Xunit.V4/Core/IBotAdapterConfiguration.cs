namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System.Collections.Generic;

    public interface IBotAdapterConfiguration
    {
        IReadOnlyCollection<UserAccessTokenConfiguration> UserAccessTokens { get; }

        void WithUserAccessTokenConfiguration(UserAccessTokenConfiguration tokenConfig);
    }
}