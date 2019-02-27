namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Threading.Tasks;

    public interface IDialogWriter<T>
    {
        Task SendActivity(T messageActivity);

        T GetStepMessageActivity(IStoryFrame<T> frame);
    }
}
