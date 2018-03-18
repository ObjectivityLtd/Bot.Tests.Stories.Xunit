namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer
{
    using System.Linq;
    using Core;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;
    using StoryModel;

    public class PerformanceStep : IStep
    {
        private const string OptionsToken = "buttons";
        private const string TokenValueKey = "value";

        public PerformanceStep()
        {
        }

        public PerformanceStep(IMessageActivity messageActivity)
        {
            this.MessageActivity = messageActivity;
            this.Message = messageActivity.Text;

            this.TrySetOptions();
        }

        public IMessageActivity MessageActivity { get; set; }

        public Actor Actor { get; set; }

        public int StepIndex { get; set; }

        public string Message { get; set; }

        public string[] Options { get; set; }

        private void TrySetOptions()
        {
            this.HandleAttachmentsWithoutContentType();
            this.HandleHeroCardAttachment();
        }

        private void HandleHeroCardAttachment()
        {
            var firstHeroCardAttachment = this.MessageActivity
                .Attachments?
                .FirstOrDefault(f => f.ContentType == HeroCard.ContentType);

            if (firstHeroCardAttachment?.Content is HeroCard heroCard)
            {
                this.Options = heroCard.Buttons.Select(b => b.Title).ToArray();
            }
        }

        private void HandleAttachmentsWithoutContentType()
            {
            var weirdAttachment = this.MessageActivity
                .Attachments
                .FirstOrDefault(att => string.IsNullOrEmpty(att.ContentType));

            if (weirdAttachment?.Content is JObject obj)
            {
                var token = obj.SelectToken(OptionsToken);
                if (token != null)
                {
                    this.Options = token.Select(item => item[TokenValueKey].ToString()).ToArray();
                }
            }
        }
    }
}