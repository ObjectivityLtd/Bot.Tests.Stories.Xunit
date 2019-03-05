namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using Configuration;
    using StoryModel;

    public interface IStoryRecorder<T>
    {
        Configuration<T> Config { get; }

        IBotRecorder<T> Bot { get; }

        IUserRecorder<T> User { get; }

        IStory<T> Rewind();
    }
}