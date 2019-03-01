namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using Config;
    using StoryModel;

    public interface IStoryRecorder<T>
    {
        IConfig<T> Config { get; }

        IBotRecorder<T> Bot { get; }

        IUserRecorder<T> User { get; }

        IStory<T> Rewind();
    }
}