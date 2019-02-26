namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Extensions
{
    using System;
    using System.Linq;
    using Microsoft.Bot.Schema;
    using Recorder;

    public static class BotStoryRecorderExtensions
    {
        /// <summary>
        /// Verifies if bot responded with choice list.
        /// <remarks>
        /// Assertion is based on activity's SuggestedActions.
        /// For other list styles please use <see cref="IBotRecorder{T}.Says(string)"/> or
        /// <see cref="IBotRecorder{T}.SendsActivity(Predicate{T})"/>
        /// </remarks>
        /// </summary>
        /// <param name="recorder">Bot recorder.</param>
        /// <param name="prompt">Prompt message text.</param>
        /// <param name="options">List of expected options.</param>
        /// <returns>Story recorder.</returns>
        public static IStoryRecorder<IMessageActivity> GivesChoice(this IBotRecorder<IMessageActivity> recorder, string prompt, string[] options)
        {
            return recorder.SendsActivity(activity =>
            {
                var actions = activity.SuggestedActions?.Actions;

                return actions != null
                       && actions.Count == options.Length
                       && options.All(x => actions.Any(a => a.Title == x));
            });
        }
    }
}
