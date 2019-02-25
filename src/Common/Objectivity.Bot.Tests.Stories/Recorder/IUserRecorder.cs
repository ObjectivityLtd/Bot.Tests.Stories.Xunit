namespace Objectivity.Bot.Tests.Stories.Recorder
{
    public interface IUserRecorder<T> : IBaseActorRecorder<T>
    {
        IStoryRecorder<T> PicksOption(int optionIndex, string optionOutputPlaceholder = null);

        IStoryRecorder<T> PicksOption(OptionNumber optionNumber, string optionOutputPlaceholder = null);
    }
}