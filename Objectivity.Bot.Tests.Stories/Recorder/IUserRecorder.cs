namespace Objectivity.Bot.Tests.Stories.Recorder
{
    public interface IUserRecorder : IBaseActorRecorder
    {
        IStoryRecorder PicksOption(int optionIndex, string optionOutputPlaceholder = null);

        IStoryRecorder PicksOption(OptionNumber optionNumber, string optionOutputPlaceholder = null);
    }
}