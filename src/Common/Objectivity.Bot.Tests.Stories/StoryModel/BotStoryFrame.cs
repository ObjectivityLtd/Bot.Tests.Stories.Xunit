namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Newtonsoft.Json.Linq;

    public class BotStoryFrame<T> : IStoryFrame<T>
    {
        public BotStoryFrame(
            ComparisonType comparisonType,
            string text = null,
            Predicate<JObject> listPredicate = null,
            Predicate<T> messageActivityPredicate = null,
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

        public Predicate<T> MessageActivityPredicate { get; }

        public IActivityBuilder<T> ActivityBuilder { get; }

        public int OptionIndex { get; }

        public string OptionOutputPlaceholder { get; }

        public string Text { get; }

        public IList<KeyValuePair<string, object>> Suggestions { get; }
    }
}