namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using System.Linq;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json.Linq;
    using StoryModel;

    public class PerformanceStep : PerformanceStep<IMessageActivity>
    {
        private const string OptionsToken = "buttons";
        private const string TokenValueKey = "value";

        public PerformanceStep()
        {
        }

        public PerformanceStep(IMessageActivity messageActivity)
            : base(messageActivity)
        {
            this.Message = messageActivity.Text;

            this.TrySetOptions();
        }

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
                .Attachments?
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