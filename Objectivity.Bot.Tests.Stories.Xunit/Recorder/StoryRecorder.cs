namespace Objectivity.Bot.Tests.Stories.Xunit.Recorder
{
    using Microsoft.Bot.Connector;
    using Stories.Recorder;

    public class StoryRecorder : StoryRecorderBase<IMessageActivity>
    {
        public static IStoryRecorder<IMessageActivity> Record()
        {
            return new StoryRecorder();
        }
    }
}
