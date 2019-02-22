namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    public enum ComparisonType
    {
        TextExact,

        TextMatchRegex,

        AttachmentListPresent,

        Option,

        TextExactWithSuggestions,

        TextMatchRegexWithSuggestions,

        Predicate,
    }
}