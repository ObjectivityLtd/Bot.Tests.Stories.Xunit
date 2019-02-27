namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System.Collections.Generic;

    public interface IDialogReader<TActivity>
    {
        List<TActivity> GetMessageActivities();

        List<TActivity> DequeueStartupMessageActivities();
    }
}
