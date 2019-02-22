namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    using System;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;

    public interface IStoryFrame
    {
        Actor Actor { get; }

        ComparisonType ComparisonType { get; }

        Predicate<JObject> ListPredicate { get; }

        Predicate<IMessageActivity> MessageActivityPredicate { get; }

        int OptionIndex { get; }

        string OptionOutputPlaceholder { get; }

        string Text { get; }
    }
}