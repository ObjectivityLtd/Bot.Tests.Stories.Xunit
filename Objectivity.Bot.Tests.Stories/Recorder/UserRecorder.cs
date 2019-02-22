namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using StoryModel;

    internal class UserRecorder<T> : IUserRecorder<T>
    {
        private readonly StoryRecorderBase<T> storyRecorder;

        public UserRecorder(StoryRecorderBase<T> storyRecorder)
        {
            this.storyRecorder = storyRecorder;
        }

        public IStoryRecorder<T> PicksOption(int optionIndex, string optionOutputPlaceholder = null)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new UserStoryFrame<T>
                {
                    Actor = Actor.User,
                    ComparisonType = ComparisonType.Option,
                    OptionIndex = optionIndex,
                    OptionOutputPlaceholder = optionOutputPlaceholder,
                });
            return this.storyRecorder;
        }

        public IStoryRecorder<T> PicksOption(OptionNumber optionNumber, string optionOutputPlaceholder = null)
        {
            return this.PicksOption((int)optionNumber, optionOutputPlaceholder);
        }

        public IStoryRecorder<T> Says(string text)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new UserStoryFrame<T> { Actor = Actor.User, ComparisonType = ComparisonType.TextExact, Text = text, });
            return this.storyRecorder;
        }
    }
}