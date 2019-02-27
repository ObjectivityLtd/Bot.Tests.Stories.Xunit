namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using Player;

    public class StoryStep<T> : IStoryStep<T>
    {
        public StoryStep(IStoryFrame<T> storyFrame, bool isDialogResultCheckupStep)
        {
            this.StoryFrame = storyFrame;
            this.Message = storyFrame.Text;
            this.Actor = storyFrame.Actor;
            this.IsDialogResultCheckupStep = isDialogResultCheckupStep;
        }

        public int StepIndex { get; set; }

        public string Message { get; set; }

        public T MessageActivity { get; set; }

        public bool IsDialogResultCheckupStep { get; }

        public StoryPlayerStepStatus Status { get; set; }

        public IStoryFrame<T> StoryFrame { get; set; }

        public Actor Actor { get; set; }
    }
}
