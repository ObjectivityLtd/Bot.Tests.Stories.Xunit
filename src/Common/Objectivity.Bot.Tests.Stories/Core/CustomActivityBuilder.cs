namespace Objectivity.Bot.Tests.Stories.Core
{
    using Objectivity.Bot.Tests.Stories.StoryModel;

    public class CustomActivityBuilder<T> : IActivityBuilder<T>
    {
        private readonly T _activity;

        public CustomActivityBuilder(T activity)
        {
            this._activity = activity;
        }

        public T Build(IStoryFrame<T> frame)
        {
            return this._activity;
        }
    }
}
