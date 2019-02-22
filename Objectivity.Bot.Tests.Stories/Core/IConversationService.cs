namespace Objectivity.Bot.Tests.Stories.Core
{
    public interface IConversationService<TActivity>
    {
        string[] LatestOptions { get; set; }

        TActivity GetToBotActivity(string text);
    }
}
