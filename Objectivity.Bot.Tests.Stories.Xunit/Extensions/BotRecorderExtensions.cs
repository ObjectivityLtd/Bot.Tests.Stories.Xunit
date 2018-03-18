namespace Objectivity.Bot.Tests.Stories.Xunit.Extensions
{
    using System.Linq;
    using Microsoft.Bot.Connector;
    using Recorder;

    public static class BotRecorderExtensions
    {
        public static IStoryRecorder Confirms(this IBotRecorder recorder, string prompt)
        {
            return recorder.GivesChoice(prompt, new[] { "Yes", "No" }); // todo: replace with prompt resources
        }

        public static IStoryRecorder GivesChoice(this IBotRecorder recorder, string prompt, string[] options)
        {
            return recorder.SendsActivity(activity =>
                activity.Attachments?
                    .FirstOrDefault(a => a.ContentType == HeroCard.ContentType)
                    ?.Content is HeroCard heroCard &&
                heroCard.Text == prompt &&
                heroCard.Buttons.Select(b => b.Title).SequenceEqual(options));
        }
    }
}