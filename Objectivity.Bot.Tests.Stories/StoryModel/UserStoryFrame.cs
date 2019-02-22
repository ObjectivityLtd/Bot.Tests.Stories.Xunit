namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System;
    using Core;
    using Newtonsoft.Json.Linq;

    public class UserStoryFrame<T> : IStoryFrame<T>
    {
        public Actor Actor { get; set; } = Actor.User;

        public ComparisonType ComparisonType { get; set; }

        public Predicate<JObject> ListPredicate { get; set; }

        public Predicate<T> MessageActivityPredicate { get; set; }

        public IActivityBuilder<T> ActivityBuilder { get; set; }

        public int OptionIndex { get; set; }

        public string OptionOutputPlaceholder { get; set; }

        public string Text { get; set; }
    }
}