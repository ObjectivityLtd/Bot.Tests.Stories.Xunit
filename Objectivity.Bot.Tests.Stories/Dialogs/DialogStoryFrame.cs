namespace Objectivity.Bot.Tests.Stories.Dialogs
{
    using System;
    using Core;
    using Newtonsoft.Json.Linq;
    using StoryModel;

    public class DialogStoryFrame<T> : IStoryFrame<T>
    {
        public DialogStoryFrame(DialogStatus dialogStatus,
            Predicate<object> resultPredicate = null,
            Type exceptionType = null)
        {
            this.DialogStatus = dialogStatus;
            this.ResultPredicate = resultPredicate;
            this.ExceptionType = exceptionType;
        }

        public IActivityBuilder<T> ActivityBuilder { get; set; }

        public DialogStatus DialogStatus { get; }

        public Predicate<object> ResultPredicate { get; }

        public Actor Actor => Actor.Bot;

        public ComparisonType ComparisonType => ComparisonType.Predicate;

        public Predicate<JObject> ListPredicate { get; }

        public Predicate<T> MessageActivityPredicate { get; }

        public int OptionIndex { get; }

        public string OptionOutputPlaceholder { get; }

        public string Text { get; }

        public Type ExceptionType { get; }
    }
}
