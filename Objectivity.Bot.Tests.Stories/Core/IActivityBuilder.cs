namespace Objectivity.Bot.Tests.Stories.Core
{
    using StoryModel;

    public interface IActivityBuilder<T>
    {
        T Build(IStoryFrame<T> frame);
    }
}
