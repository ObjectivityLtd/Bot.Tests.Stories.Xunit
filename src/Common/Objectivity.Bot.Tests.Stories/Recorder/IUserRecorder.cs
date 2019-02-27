namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused methods are planned to be used outside the framework")]
    public interface IUserRecorder<T> : IBaseActorRecorder<T>
    {
        IStoryRecorder<T> PicksOption(int optionIndex);

        IStoryRecorder<T> PicksOption(OptionNumber optionNumber);
    }
}