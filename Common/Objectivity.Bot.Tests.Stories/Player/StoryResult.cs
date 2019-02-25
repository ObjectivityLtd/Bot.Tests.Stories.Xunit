namespace Objectivity.Bot.Tests.Stories.Player
{
    using System.Collections.Generic;

    public class StoryResult : IStoryResult
    {
        public IDictionary<string, object> OutputValues { get; } = new Dictionary<string, object>();
    }
}