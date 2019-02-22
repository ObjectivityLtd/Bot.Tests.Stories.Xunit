namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using StoryModel;

    internal class UserRecorder : IUserRecorder
    {
        private readonly StoryRecorder storyRecorder;

        public UserRecorder(StoryRecorder storyRecorder)
        {
            this.storyRecorder = storyRecorder;
        }

        public IStoryRecorder PicksOption(int optionIndex, string optionOutputPlaceholder = null)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new UserStoryFrame
                {
                    Actor = Actor.User,
                    ComparisonType = ComparisonType.Option,
                    OptionIndex = optionIndex,
                    OptionOutputPlaceholder = optionOutputPlaceholder,
                });
            return this.storyRecorder;
        }

        public IStoryRecorder PicksOption(OptionNumber optionNumber, string optionOutputPlaceholder = null)
        {
            return this.PicksOption((int)optionNumber, optionOutputPlaceholder);
        }

        public IStoryRecorder Says(string text)
        {
            this.storyRecorder.Story.AddStoryFrame(
                new UserStoryFrame { Actor = Actor.User, ComparisonType = ComparisonType.TextExact, Text = text, });
            return this.storyRecorder;
        }
    }
}