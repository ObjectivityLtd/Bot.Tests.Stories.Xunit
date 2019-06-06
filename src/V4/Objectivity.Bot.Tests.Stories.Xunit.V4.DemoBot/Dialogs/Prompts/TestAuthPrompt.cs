namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs.Prompts
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;

    public class TestAuthPrompt : ActivityPrompt
    {
        public const string AuthRequestEventName = "tokens/request";
        public const string AuthResponseEventName = "tokens/response";

        public TestAuthPrompt(string dialogId, PromptValidator<Activity> validator)
            : base(dialogId, validator)
        {
        }

        protected override Task<PromptRecognizerResult<Activity>> OnRecognizeAsync(
            ITurnContext turnContext,
            IDictionary<string, object> state,
            PromptOptions options,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new PromptRecognizerResult<Activity>();
            var activity = turnContext.Activity;

            if (activity.Type == ActivityTypes.Event)
            {
                var ev = activity.AsEventActivity();

                if (ev.Name == AuthResponseEventName)
                {
                    result.Succeeded = true;
                    result.Value = turnContext.Activity;
                }
            }

            return Task.FromResult(result);
        }
    }
}
