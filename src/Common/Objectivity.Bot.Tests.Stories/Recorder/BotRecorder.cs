namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using StoryModel;

    public class BotRecorder<T> : IBotRecorder<T>
    {
        public BotRecorder(StoryRecorderBase<T> storyRecorder)
        {
            this.StoryRecorder = storyRecorder;
        }

        protected StoryRecorderBase<T> StoryRecorder { get; }

        public IStoryRecorder<T> ListsOptions(Predicate<JObject> listPredicate = null)
        {
            this.StoryRecorder.Story.AddStoryFrame(
                new BotStoryFrame<T>(ComparisonType.AttachmentListPresent, listPredicate: listPredicate));
            return this.StoryRecorder;
        }

        public IStoryRecorder<T> ListsOptions(string title, string[] options)
        {
            this.StoryRecorder.Story.AddStoryFrame(
                new BotStoryFrame<T>(
                    ComparisonType.AttachmentListPresent,
                    listPredicate: list =>
                        this.TitleEquals(list, title) &&
                        this.ContainsOptions(list, options)));

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> ListsOptions(string title, string subtitle, string[] options)
        {
            this.StoryRecorder.Story.AddStoryFrame(
                new BotStoryFrame<T>(
                    ComparisonType.AttachmentListPresent,
                    listPredicate: list =>
                        this.TitleEquals(list, title) &&
                        this.SubtitleEquals(list, subtitle) &&
                        this.ContainsOptions(list, options)));

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> ListsOptionsIncluding(params string[] options)
        {
            this.StoryRecorder.Story.AddStoryFrame(
                new BotStoryFrame<T>(
                    ComparisonType.AttachmentListPresent,
                    listPredicate: list => this.ContainsOptions(list, options)));

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> Says(string text, IList<KeyValuePair<string, object>> suggestions)
        {
            this.StoryRecorder.Story.AddStoryFrame(
                new BotStoryFrame<T>(
                    ComparisonType.TextExactWithSuggestions,
                    text,
                    suggestions: suggestions));

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> SaysSomethingLike(string pattern, IList<KeyValuePair<string, object>> suggestions = null)
        {
            this.StoryRecorder.Story.AddStoryFrame(
                new BotStoryFrame<T>(
                    suggestions == null ? ComparisonType.TextMatchRegex : ComparisonType.TextMatchRegexWithSuggestions,
                    pattern,
                    suggestions: suggestions));

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> Says(string text)
        {
            this.StoryRecorder.Story.AddStoryFrame(new BotStoryFrame<T>(ComparisonType.TextExact, text));

            return this.StoryRecorder;
        }

        public IStoryRecorder<T> SendsActivity(Predicate<T> predicate)
        {
            this.StoryRecorder.Story.AddStoryFrame(new BotStoryFrame<T>(ComparisonType.Predicate, messageActivityPredicate: predicate));

            return this.StoryRecorder;
        }

        private bool StringTokenPredicate(JObject list, string tokenName, string value)
        {
            var tokenValue = list.SelectToken(tokenName)?.ToString() ?? string.Empty;

            return tokenValue.Equals(value, StringComparison.InvariantCulture);
        }

        private bool TitleEquals(JObject list, string title)
        {
            return this.StringTokenPredicate(list, "title", title);
        }

        private bool SubtitleEquals(JObject list, string subtitle)
        {
            return this.StringTokenPredicate(list, "subtitle", subtitle);
        }

        private bool ContainsOptions(JObject list, string[] options)
        {
            var availableOptions = list.SelectToken("buttons").Select(item => item["value"].ToString())
                .ToList();

            return options.All(option => availableOptions.Contains(option));
        }
    }
}