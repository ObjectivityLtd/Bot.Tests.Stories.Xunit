namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;

    public class BotStoryFrame : IStoryFrame
    {
        public BotStoryFrame(
            ComparisonType comparisonType,
            string text = null,
            Predicate<JObject> listPredicate = null,
            Predicate<IMessageActivity> messageActivityPredicate = null,
            IList<KeyValuePair<string, object>> suggestions = null)
        {
            this.Actor = Actor.Bot;
            this.ComparisonType = comparisonType;
            this.Text = text;
            this.ListPredicate = listPredicate;
            this.Suggestions = suggestions;
            this.MessageActivityPredicate = messageActivityPredicate;
        }

        public Actor Actor { get; }

        public ComparisonType ComparisonType { get; }

        public Predicate<JObject> ListPredicate { get; }

        public Predicate<IMessageActivity> MessageActivityPredicate { get; }

        public int OptionIndex { get; }

        public string OptionOutputPlaceholder { get; }

        public string Text { get; }

        public IList<KeyValuePair<string, object>> Suggestions { get; }
    }
}