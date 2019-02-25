namespace Objectivity.Bot.Tests.Stories.Recorder
{
    public interface IBaseActorRecorder<T>
    {
        IStoryRecorder<T> Says(string text);
    }
}