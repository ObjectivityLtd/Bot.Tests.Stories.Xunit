namespace Objectivity.Bot.Tests.Stories.Xunit.Extensions
{
    using System;
    using Core;
    using Microsoft.Bot.Connector;
    using Stories.Recorder;
    using StoryModel;

    public static class StoryRecorderExtensions
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

            story.AddStoryFrame(new DialogStoryFrame(resultType, resultPredicate,  exceptionType));

            return story;
        }
    }
}
