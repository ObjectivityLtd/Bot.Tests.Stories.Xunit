namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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

        [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Property forced by implemented interface.")]
        public Predicate<JObject> ListPredicate { get; }

        [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Property forced by implemented interface.")]
        public Predicate<IMessageActivity> MessageActivityPredicate { get; }

        [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Property forced by implemented interface.")]
        public int OptionIndex { get; }

        [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Property forced by implemented interface.")]
        public string Text { get; }

        public Type ExceptionType { get; }
    }
}
