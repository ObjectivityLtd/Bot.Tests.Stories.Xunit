namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Threading.Tasks;
    using Dialogs;
    using Extensions;
    using global::Xunit;
    using Recorder;

    public class PromptChoiceDialogTests : DialogUnitTestBase<PromptChoiceDialog>
    {
        [Theory]
        [InlineData("First")]
        [InlineData("Second")]
        [InlineData("Third")]
        public async Task DialogWithChoice_UserSelectsAny_BotSaysWhatYouChose(string input)
        {
            var story = StoryRecorder
                .Record()
                .Bot.GivesChoice("Choose one of the options", new[] { "First", "Second", "Third" })
                .User.Says(input)
                .Bot.Says("You chose " + input)
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task DialogWithChoice_UserRespondSomethingElse_BotTellsToAnswerTheQuestion()
        {
            var story = StoryRecorder
                .Record()
                .Bot.GivesChoice("Choose one of the options", new[] { "First", "Second", "Third" })
                .User.Says("Fourth")
                .Bot.GivesChoice("Choose one of the options", new[] { "First", "Second", "Third" })
                .Rewind();

            await this.Play(story);
        }
    }
}