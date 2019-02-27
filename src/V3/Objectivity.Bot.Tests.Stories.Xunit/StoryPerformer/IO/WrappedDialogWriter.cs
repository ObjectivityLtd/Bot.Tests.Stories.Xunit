namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer.IO
{
    using System.Threading.Tasks;
    using Core;
    using Exceptions;
    using Microsoft.Bot.Connector;
    using Moq;
    using Stories.Core;
    using StoryModel;

    public class WrappedDialogWriter : IDialogWriter<IMessageActivity>
    {
        private readonly IScopeContext scopeContext;
        private readonly IConversationService<IMessageActivity> conversationService;

        public WrappedDialogWriter(IScopeContext scopeContext, IConversationService<IMessageActivity> conversationService)
        {
            this.scopeContext = scopeContext;
            this.conversationService = conversationService;
        }

        public async Task SendActivity(IMessageActivity messageActivity)
        {
            try
            {
                await TestConversation.SendAsync(this.scopeContext.Scope, messageActivity);
            }
            catch (MockException mockException)
            {
                var message = messageActivity?.Text;

                throw new UnmatchedUtteranceException(
                    $"Error while sending user message - matching intent couldn't be found. Check if unit test registers intent for the following utterance: {message}.",
                    mockException);
            }
        }

        public IMessageActivity GetStepMessageActivity(IStoryFrame<IMessageActivity> frame)
        {
            var message = this.GetUserStepMessage(frame);

            return this.conversationService.GetToBotActivity(message);
        }

        private string GetUserStepMessage(IStoryFrame<IMessageActivity> frame)
        {
            switch (frame.ComparisonType)
            {
                case ComparisonType.Option:
                    return this.GetUserOptionMessage(frame);
                default:
                    return frame.Text;
            }
        }

        private string GetUserOptionMessage(IStoryFrame<IMessageActivity> frame)
        {
            if (this.conversationService.LatestOptions == null)
            {
                throw new MissingOptionsException();
            }

            var index = frame.OptionIndex;

            if (index < 0 || this.conversationService.LatestOptions.Length <= index)
            {
                throw new OptionsIndexOutOfRangeException(index, this.conversationService.LatestOptions.Length);
            }

            return this.conversationService.LatestOptions[index];
        }
    }
}
