namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    public enum TestContainerBuilderOptions
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused member is planned to be used outside the framework")]
        None = 0,
        Reflection = 1,
        ScopedQueue = 2,
        MockConnectorFactory = 4,
        ResolveDialogFromContainer = 8,
        LastWriteWinsCachingBotDataStore = 16
    }
}
