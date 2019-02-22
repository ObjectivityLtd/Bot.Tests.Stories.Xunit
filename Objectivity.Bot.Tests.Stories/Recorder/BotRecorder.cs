namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;
    using StoryModel;

    internal class BotRecorder : IBotRecorder
    {
        private readonly StoryRecorder storyRecorder;

        public BotRecorder(StoryRecorder storyRecorder)
        {
            this.storyRecorder = storyRecorder;
        }

        public IStoryRecorder ListsOptions(Predicate<JObject> listPredicate = null)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new BotStoryFrame(ComparisonType.AttachmentListPresent, listPredicate: listPredicate));
            return this.storyRecorder;
        }

        public IStoryRecorder ListsOptionsIncluding(params string[] options)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new BotStoryFrame(
                    ComparisonType.AttachmentListPresent,
                    listPredicate: list =>
                    {
                        var availableOptions = list.SelectToken("buttons").Select(item => item["value"].ToString())
                            .ToList();

                        return options.All(option => availableOptions.Contains(option));
                    }));
            return this.storyRecorder;
        }

        public IStoryRecorder Says(string text, IList<KeyValuePair<string, object>> suggestions)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new BotStoryFrame(
                    ComparisonType.TextExactWithSuggestions,
                    text,
                    suggestions: suggestions));

            return this.storyRecorder;
        }

        public IStoryRecorder SaysSomethingLike(string pattern, IList<KeyValuePair<string, object>> suggestions = null)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new BotStoryFrame(
                    suggestions == null ? ComparisonType.TextMatchRegex : ComparisonType.TextMatchRegexWithSuggestions,
                    pattern,
                    suggestions: suggestions));

            return this.storyRecorder;
        }

        public IStoryRecorder Says(string text)
        {
            this.storyRecorder.Story.AddStoryFrame(new BotStoryFrame(ComparisonType.TextExact, text));

            return this.storyRecorder;
        }

        public IStoryRecorder SendsActivity(Predicate<IMessageActivity> predicate)
        {
            this.storyRecorder.Story.AddStoryFrame(new BotStoryFrame(ComparisonType.Predicate, messageActivityPredicate: predicate));

            return this.storyRecorder;
        }
    }
}