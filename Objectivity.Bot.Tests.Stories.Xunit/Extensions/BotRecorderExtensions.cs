namespace Objectivity.Bot.Tests.Stories.Xunit.Extensions
{
    using System.Linq;
    using Microsoft.Bot.Connector;
    using Recorder;

    public static class BotRecorderExtensions
    {
        public static IStoryRecorder Confirms(this IBotRecorder recorder, string prompt)
        {
            return recorder.SendsActivity(activity =>
                activity.Attachments?
                    .FirstOrDefault(a => a.ContentType == HeroCard.ContentType)
                    ?.Content is HeroCard heroCard &&
                heroCard.Text == prompt &&
                heroCard.Buttons.Count == 2 &&
                heroCard.Buttons[0].Title == "Yes" && // todo: replace with prompt resources
                heroCard.Buttons[1].Title == "No");
        }
    }
}
