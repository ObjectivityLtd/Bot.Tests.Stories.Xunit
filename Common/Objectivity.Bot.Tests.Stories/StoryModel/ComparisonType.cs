namespace Objectivity.Bot.Tests.Stories.StoryModel
{
    public enum ComparisonType
    {
        None,

        TextExact,

        TextMatchRegex,

        AttachmentListPresent,

        Option,

        TextExactWithSuggestions,

        TextMatchRegexWithSuggestions,

        Predicate,
    }
}