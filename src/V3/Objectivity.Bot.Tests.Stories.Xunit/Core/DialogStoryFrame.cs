namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;
    using Stories.Core;
    using StoryModel;

    public class DialogStoryFrame : IStoryFrame<IMessageActivity>
    {
        public DialogStoryFrame(
            DialogStatus dialogStatus,
            Predicate<object> resultPredicate = null,
            Type exceptionType = null)
        {
            this.DialogStatus = dialogStatus;
            this.ResultPredicate = resultPredicate;
            this.ExceptionType = exceptionType;
        }

        public IActivityBuilder<IMessageActivity> ActivityBuilder { get; set; }

        public DialogStatus DialogStatus { get; }

        public Predicate<object> ResultPredicate { get; }

        public Actor Actor => Actor.Bot;

        public ComparisonType ComparisonType => ComparisonType.Predicate;

        public Predicate<JObject> ListPredicate { get; }

        public Predicate<IMessageActivity> MessageActivityPredicate { get; }

        public int OptionIndex { get; }

        public string OptionOutputPlaceholder { get; }

        public string Text { get; }

        public Type ExceptionType { get; }
    }
}
