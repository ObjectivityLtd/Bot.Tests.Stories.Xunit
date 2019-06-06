namespace Objectivity.Bot.Tests.Stories.Core
{
    using Objectivity.Bot.Tests.Stories.StoryModel;

    public class CustomActivityBuilder<T> : IActivityBuilder<T>
    {
        private readonly T activity;

        public CustomActivityBuilder(T activity)
        {
            this.activity = activity;
        }

        public T Build(IStoryFrame<T> frame)
        {
            return this.activity;
        }
    }
}
