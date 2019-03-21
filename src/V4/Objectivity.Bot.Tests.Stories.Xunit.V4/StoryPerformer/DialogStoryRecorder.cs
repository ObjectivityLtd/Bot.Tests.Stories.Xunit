namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using Microsoft.Bot.Schema;
    using Recorder;
    using StoryModel;

    public class DialogStoryRecorder : StoryRecorderBase<IMessageActivity>
    {
        private DialogStoryRecorder()
        {
        }

        public static IStoryRecorder<IMessageActivity> Record()
        {
            var recorder = new DialogStoryRecorder();

            recorder.Story.AddStoryFrame(new UserStoryFrame<IMessageActivity>
            {
                Text = null,
                ComparisonType = ComparisonType.None,
            });

            return recorder;
        }
    }
}