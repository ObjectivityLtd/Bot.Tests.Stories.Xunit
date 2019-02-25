namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using StoryModel;

    public interface IStoryRecorder<T>
    {
        IBotRecorder<T> Bot { get; }

        IUserRecorder<T> User { get; }

        IStory<T> Rewind();
    }
}