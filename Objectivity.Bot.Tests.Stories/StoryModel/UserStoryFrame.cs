namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;

    public class UserStoryFrame : IStoryFrame
    {
        public Actor Actor { get; set; }

        public ComparisonType ComparisonType { get; set; }

        public Predicate<JObject> ListPredicate { get; set; }

        public Predicate<IMessageActivity> MessageActivityPredicate { get; set; }

        public int OptionIndex { get; set; }

        public string OptionOutputPlaceholder { get; set; }

        public string Text { get; set; }
    }
}