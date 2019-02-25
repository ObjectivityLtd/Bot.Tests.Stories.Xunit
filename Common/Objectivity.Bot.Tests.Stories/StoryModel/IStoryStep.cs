namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    public interface IStoryStep<T>
    {
        Actor Actor { get; set; }

        int StepIndex { get; set; }

        string Message { get; set; }

        T MessageActivity { get; set; }
    }
}