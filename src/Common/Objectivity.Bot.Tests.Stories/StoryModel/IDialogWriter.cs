namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer.IO
{
    using System.Threading.Tasks;
    using StoryModel;

    public interface IDialogWriter<T>
    {
        Task SendActivity(T messageActivity);

        T GetStepMessageActivity(IStoryFrame<T> frame);
    }
}
