namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Asserts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using global::Xunit;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using StoryModel;

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local", Justification = "Methods with precondition check only parameters created for code clarity.")]
    public static class StepAsserts
    {
        private const string MessageType = "message";

        public static void AssertStep(StoryStep<IMessageActivity> storyStep, PerformanceStep<IMessageActivity> performanceStep, string[] options = null)
        {
            switch (storyStep.Actor)
            {
                case Actor.Bot:
                    AssertBotStepMessage(storyStep, performanceStep);
                    break;
                case Actor.User:
                    AssertUserStepMessage(storyStep, performanceStep, options);
                    break;
            }
        }

        private static void AssertBotStepMessage(StoryStep<IMessageActivity> storyStep, PerformanceStep<IMessageActivity> performanceStep)
        {
            var message = performanceStep.MessageActivity;
            var frame = storyStep.StoryFrame;

            switch (frame.ComparisonType)
            {
                case ComparisonType.None:
                    break;

                case ComparisonType.TextExact:
                    ProcessFrameTextExact(frame, performanceStep.MessageActivity.Type, performanceStep.Message);
                    break;

                case ComparisonType.TextMatchRegex:
                    ProcessBotFrameTextMatchRegex(frame, message);
                    break;

                case ComparisonType.AttachmentListPresent:
                    ProcessBotFrameListPresent(frame, message);
                    break;

                case ComparisonType.TextExactWithSuggestions:
                    ProcessBotFrameTextWithSuggestions(frame, message);
                    break;

                case ComparisonType.TextMatchRegexWithSuggestions:
                    ProcessBotFrameTextMatchRegexWithSuggestions(frame, message);
                    break;
                case ComparisonType.Predicate:
                    ProcessBotFramePredicate(frame, message);
                    break;
                default:
                    var reasonMessage = string.Format(
                        CultureInfo.InvariantCulture,
                        "Comparison type {0} is not supported for bot frame.",
                        frame.ComparisonType);

                    throw new ArgumentOutOfRangeException(nameof(frame.ComparisonType), reasonMessage);
            }
        }

        private static void AssertUserStepMessage(StoryStep<IMessageActivity> storyStep, PerformanceStep<IMessageActivity> performanceStep, IReadOnlyList<string> options = null)
        {
            var frame = storyStep.StoryFrame;

            switch (frame.ComparisonType)
            {
                case ComparisonType.None:
                    break;
                case ComparisonType.TextExact:
                    ProcessFrameTextExact(frame, performanceStep.MessageActivity.Type, performanceStep.Message);
                    break;
                case ComparisonType.Option:
                    AssertUserFrameOption(frame, performanceStep.MessageActivity, options);
                    break;
                default:
                    var reasonMessage = string.Format(
                        CultureInfo.InvariantCulture,
                        "Comparison type {0} is not supported for user frame.",
                        frame.ComparisonType);

                    throw new ArgumentOutOfRangeException(nameof(frame.ComparisonType), reasonMessage);
            }
        }

        private static void AssertUserFrameOption(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message, IReadOnlyList<string> options)
        {
            Assert.NotEmpty(options);

            var optionValue = options[storyFrame.OptionIndex];

            Assert.Equal(optionValue, message.Text);
        }

        private static void ProcessFrameTextExact(IStoryFrame<IMessageActivity> storyFrame, string messageType, string message)
        {
            Assert.NotNull(message);
            Assert.Equal(MessageType, messageType);
            Assert.Equal(storyFrame.Text, message);
        }

        private static void ProcessBotFrameTextMatchRegex(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message)
        {
            Assert.NotNull(message);
            Assert.Equal(MessageType, message.Type);
            Assert.Matches(storyFrame.Text, message.Text);
        }

        private static void ProcessBotFramePredicate(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message)
        {
            Assert.True(storyFrame.MessageActivityPredicate(message));
        }

        private static void ProcessBotFrameTextMatchRegexWithSuggestions(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message)
        {
            ProcessBotFrameTextMatchRegex(storyFrame, message);
            AssertSuggestions(storyFrame, message);
        }

        private static void ProcessBotFrameTextWithSuggestions(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message)
        {
            ProcessFrameTextExact(storyFrame, message.Type, message.Text);
            AssertSuggestions(storyFrame, message);
        }

        private static void AssertSuggestions(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message)
        {
            var botStoryFrame = storyFrame as BotStoryFrame<IMessageActivity>;

            Assert.NotNull(botStoryFrame);
            Assert.Equal(botStoryFrame.Suggestions, message.SuggestedActions.Actions.Select(s => new KeyValuePair<string, object>(s.Title, s.Value)));
        }

        private static void ProcessBotFrameListPresent(IStoryFrame<IMessageActivity> storyFrame, IMessageActivity message)
        {
            Assert.NotNull(message);
            Assert.Equal(1, message.Attachments.Count);
            Assert.NotNull(message.Attachments[0].Content);

            var listJson = JObject.Parse(JsonConvert.SerializeObject(message.Attachments[0].Content));

            if (storyFrame.ListPredicate != null)
            {
                Assert.True(storyFrame.ListPredicate(listJson), "List contains expected item");
            }
        }
    }
}
