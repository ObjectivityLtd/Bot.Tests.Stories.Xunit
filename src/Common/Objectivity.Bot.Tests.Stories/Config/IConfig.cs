namespace Objectivity.Bot.Tests.Stories.Config
{
    using Recorder;

    public interface IConfig<T>
    {
        IStoryRecorder<T> UseChannel(string channelId);
    }
}
