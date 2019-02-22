namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using StoryModel;

    public interface IStoryRecorder
    {
        IBotRecorder Bot { get; }

        IUserRecorder User { get; }

        IStory Rewind();
    }
}