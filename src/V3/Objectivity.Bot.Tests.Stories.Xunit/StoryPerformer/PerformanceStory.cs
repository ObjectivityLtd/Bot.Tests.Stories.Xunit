namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer
{
    using Microsoft.Bot.Connector;
    using StoryModel;

    public class PerformanceStory : PerformanceStory<IMessageActivity>
    {
        private const string TypingMessageType = "typing";

        protected override PerformanceStep<IMessageActivity> GetPerformanceStep(IMessageActivity message, Actor actor)
        {
            if (message.Type == TypingMessageType || message.Text == Constants.WrapperStartMessage)
            {
                return null;
            }

            var performanceStep = new PerformanceStep(message)
            {
                Actor = actor
            };

            return performanceStep;
        }
    }
}
