namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Bot.Connector;
    using Newtonsoft.Json.Linq;

    public interface IBotRecorder : IBaseActorRecorder
    {
        IStoryRecorder ListsOptions(Predicate<JObject> listPredicate = null);

        IStoryRecorder SendsActivity(Predicate<IMessageActivity> predicate);

        IStoryRecorder ListsOptionsIncluding(params string[] options);

        IStoryRecorder SaysSomethingLike(string pattern, IList<KeyValuePair<string, object>> suggestions = null);

        IStoryRecorder Says(string pattern, IList<KeyValuePair<string, object>> suggestions);
    }
}