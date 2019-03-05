namespace Objectivity.Bot.Tests.Stories.Configuration
{
    using Recorder;

    public interface IConfigurationRecorder<T>
    {
        IStoryRecorder<T> UseChannel(string channelId);
    }
}
