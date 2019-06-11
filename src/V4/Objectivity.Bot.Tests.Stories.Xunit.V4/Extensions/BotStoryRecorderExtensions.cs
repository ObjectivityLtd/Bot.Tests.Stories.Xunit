namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.Bot.Schema;
    using Recorder;

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused members are planned to be used outside the framework")]
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

        /// <summary>
        /// Verifies if bot sent an event.
        /// <remarks>
        /// Assertion is based on activity type and name. Name verification is case sensitive.
        /// </remarks>
        /// </summary>
        /// <param name="recorder">Bot recorder.</param>
        /// <param name="name">Event name.</param>
        /// <returns>Story recorder.</returns>
        public static IStoryRecorder<IMessageActivity> SendsEvent(
            this IBotRecorder<IMessageActivity> recorder,
            string name)
        {
            return recorder.SendsActivity(activity => activity != null
                                                      && activity.Type == ActivityTypes.Event
                                                      && activity.AsEventActivity()?.Name == name);
        }

        /// <summary>
        /// Verifies if bot sent an event.
        /// <remarks>
        /// Assertion is based on event activity predicate.
        /// </remarks>
        /// </summary>
        /// <param name="recorder">Bot recorder.</param>
        /// <param name="eventPredicate">Predicate used for event assertion.</param>
        /// <returns>Story recorder.</returns>
        public static IStoryRecorder<IMessageActivity> SendsEvent(
            this IBotRecorder<IMessageActivity> recorder,
            Func<IEventActivity, bool> eventPredicate)
        {
            return recorder.SendsActivity(activity => activity != null
                                                      && activity.Type == ActivityTypes.Event
                                                      && eventPredicate(activity.AsEventActivity()));
        }
    }
}
