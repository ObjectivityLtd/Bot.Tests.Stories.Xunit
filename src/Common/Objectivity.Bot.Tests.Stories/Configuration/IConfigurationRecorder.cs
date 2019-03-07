namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Recorder;

    public interface IConfigurationRecorder<T>
    {
        IStoryRecorder<T> UseChannel(string channelId);

        IStoryRecorder<T> RegisterService(Action<ServiceCollection> registration);
    }
}
