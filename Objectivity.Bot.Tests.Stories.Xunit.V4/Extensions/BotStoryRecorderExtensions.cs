namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Extensions
{
    using System;
    using System.Linq;
    using Microsoft.Bot.Schema;
    using Recorder;

    public static class BotStoryRecorderExtensions
    {
        public static IStoryRecorder<IMessageActivity> GivesChoice(this IBotRecorder<IMessageActivity> recorder, string prompt, string[] options)
        {
            return recorder.SendsActivity(activity =>
            {
                var optionsPart = activity.Text?.Replace(prompt, string.Empty)?.Trim();

                var isValid = !string.IsNullOrWhiteSpace(optionsPart) && 
                              options.All(x => optionsPart.Contains(x, StringComparison.InvariantCultureIgnoreCase));

                if (isValid)
                {
                    var replaceMarks = new[] { " ", ",", "(", ")", ".", "\n", "or", };

                    var result = options.Aggregate(optionsPart, (current, next) => current.Replace(next, string.Empty));
                    result = replaceMarks.Aggregate(result, (current, next) => current.Replace(next, string.Empty));

                    for (int i = 1; i <= options.Length; i++)
                    {
                        result = result.Replace($"{i}", string.Empty);
                    }

                    isValid = string.IsNullOrWhiteSpace(result);
                }

                return isValid;
            });
        }
    }
}
