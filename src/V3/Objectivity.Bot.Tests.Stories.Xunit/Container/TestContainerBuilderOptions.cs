namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using System;

    [Flags]
    public enum TestContainerBuilderOptions
    {
        None = 0,
        Reflection = 1,
        ScopedQueue = 2,
        MockConnectorFactory = 4,
        ResolveDialogFromContainer = 8,
        LastWriteWinsCachingBotDataStore = 16
    }
}
