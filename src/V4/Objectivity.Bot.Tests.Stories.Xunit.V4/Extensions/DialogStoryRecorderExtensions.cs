namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Extensions
{
    using System;
    using Dialogs;
    using Microsoft.Bot.Schema;
    using Recorder;
    using StoryModel;

    public static class DialogStoryRecorderExtensions
    {
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

        private static IStory<IMessageActivity> GetResultStory(IStoryRecorder<IMessageActivity> storyRecorder, DialogStatus resultType, Predicate<object> resultPredicate = null, Type exceptionType = null)
        {
            var story = storyRecorder.Rewind();

            story.AddStoryFrame(new DialogStoryFrame<IMessageActivity>(resultType, resultPredicate, exceptionType));

            return story;
        }
    }
}
