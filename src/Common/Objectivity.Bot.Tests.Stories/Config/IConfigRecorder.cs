namespace Objectivity.Bot.Tests.Stories.Config
{
    using Recorder;

    public interface IConfigRecorder<T>
    {
        IStoryRecorder<T> UseChannel(string channelId);
    }
}
