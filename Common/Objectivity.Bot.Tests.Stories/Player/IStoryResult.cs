namespace Objectivity.Bot.Tests.Stories.Player
{
    using System.Collections.Generic;

    public interface IStoryResult
    {
        IDictionary<string, object> OutputValues { get; }
    }
}