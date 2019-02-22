namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System;
    using Core;
    using Newtonsoft.Json.Linq;

    public interface IStoryFrame<T>
    {
        Actor Actor { get; }

        ComparisonType ComparisonType { get; }

        Predicate<JObject> ListPredicate { get; }

        Predicate<T> MessageActivityPredicate { get; }

        IActivityBuilder<T> ActivityBuilder { get; }

        int OptionIndex { get; }

        string OptionOutputPlaceholder { get; }

        string Text { get; }
    }
}