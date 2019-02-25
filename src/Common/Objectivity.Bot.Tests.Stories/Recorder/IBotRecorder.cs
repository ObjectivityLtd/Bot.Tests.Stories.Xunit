namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    public interface IBotRecorder<T> : IBaseActorRecorder<T>
    {
        IStoryRecorder<T> ListsOptions(Predicate<JObject> listPredicate = null);

        IStoryRecorder<T> ListsOptions(string title, string[] options);

        IStoryRecorder<T> ListsOptions(string title, string subtitle, string[] options);

        IStoryRecorder<T> ListsOptionsIncluding(params string[] options);

        IStoryRecorder<T> SendsActivity(Predicate<T> predicate);

        IStoryRecorder<T> SaysSomethingLike(string pattern, IList<KeyValuePair<string, object>> suggestions = null);

        IStoryRecorder<T> Says(string pattern, IList<KeyValuePair<string, object>> suggestions);
    }
}