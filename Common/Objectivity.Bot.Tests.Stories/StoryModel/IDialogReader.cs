namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer.IO
{
    using System.Collections.Generic;

    public interface IDialogReader<TActivity>
    {
        List<TActivity> GetMessageActivities();

        List<TActivity> DequeueStartupMessageActivities();
    }
}
