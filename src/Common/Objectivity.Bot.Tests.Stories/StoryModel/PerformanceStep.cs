namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    public class PerformanceStep<T> : IStoryStep<T>
    {
        protected const string OptionsToken = "buttons";
        protected const string TokenValueKey = "value";

        public PerformanceStep(T messageActivity)
        {
            this.MessageActivity = messageActivity;
        }

        public T MessageActivity { get; set; }

        public Actor Actor { get; set; }

        public int StepIndex { get; set; }

        public string Message { get; set; }

        public string[] Options { get; set; }
    }
}
