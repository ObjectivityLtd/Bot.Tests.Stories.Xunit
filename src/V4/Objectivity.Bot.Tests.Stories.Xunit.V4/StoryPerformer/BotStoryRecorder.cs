namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using Core;
    using Microsoft.Bot.Schema;
    using Recorder;
    using StoryModel;

    public class BotStoryRecorder : StoryRecorderBase<IMessageActivity>
    {
        private BotStoryRecorder()
        {
        }

        public static IStoryRecorder<IMessageActivity> Record()
        {
            var recorder = new BotStoryRecorder();

            recorder.Story.AddStoryFrame(new UserStoryFrame<IMessageActivity>
            {
                Text = null,
                ComparisonType = ComparisonType.None,
                ActivityBuilder = new ConversationUpdateActivityBuilder(),
            });

            return recorder;
        }
    }
}