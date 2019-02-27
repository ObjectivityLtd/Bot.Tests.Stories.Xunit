namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Dialogs;
    using Microsoft.Bot.Schema;
    using Recorder;
    using StoryModel;

    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused members are planned to be used outside the framework")]
    public static class DialogStoryRecorderExtensions
    {
        public static IStory<IMessageActivity> DialogDone(this IStoryRecorder<IMessageActivity> recorder)
        {
            return GetResultStory(recorder, DialogStatus.Finished);
        }

        public static IStory<IMessageActivity> DialogDoneWithResult(this IStoryRecorder<IMessageActivity> recorder, Predicate<object> resultPredicate)
        {
            return GetResultStory(recorder, DialogStatus.Finished, resultPredicate);
        }

        public static IStory<IMessageActivity> DialogDoneWithResult<T>(this IStoryRecorder<IMessageActivity> recorder, Predicate<T> resultPredicate)
        {
            return GetResultStory(recorder, DialogStatus.Finished, resultObject =>
            {
                T result;

                try
                {
                    result = (T)resultObject;
                }
                catch
                {
                    var message =
                        $"Unable to cast result of type '{resultObject.GetType().Name}' to type '{typeof(T).Name}'.";

                    throw new FormatException(message);
                }

                return resultPredicate(result);
            });
        }

        public static IStory<IMessageActivity> DialogFailed(this IStoryRecorder<IMessageActivity> recorder)
        {
            return GetResultStory(recorder, DialogStatus.Failed);
        }

        public static IStory<IMessageActivity> DialogFailedWithExceptionOfType<TExceptionType>(this IStoryRecorder<IMessageActivity> recorder)
        {
            return GetResultStory(recorder, DialogStatus.Failed, null, typeof(TExceptionType));
        }

        private static IStory<IMessageActivity> GetResultStory(IStoryRecorder<IMessageActivity> storyRecorder, DialogStatus resultType, Predicate<object> resultPredicate = null, Type exceptionType = null)
        {
            var story = storyRecorder.Rewind();

            story.AddStoryFrame(new DialogStoryFrame<IMessageActivity>(resultType, resultPredicate,  exceptionType));

            return story;
        }
    }
}
