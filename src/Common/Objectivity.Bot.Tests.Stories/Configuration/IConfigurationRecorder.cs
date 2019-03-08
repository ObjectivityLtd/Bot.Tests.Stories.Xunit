namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused methods are used outside the framework")]
    public interface IConfigurationRecorder<T>
    {
        IStoryRecorder<T> UseChannel(string channelId);

        IStoryRecorder<T> RegisterService(Action<ServiceCollection> registration);
    }
}
