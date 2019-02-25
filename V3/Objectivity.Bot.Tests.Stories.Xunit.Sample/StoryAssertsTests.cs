namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Asserts;
    using Dialogs;
    using global::Xunit;
    using Recorder;
    using Xunit;

    public class StoryAssertsTests : DialogUnitTestBase<BotInitiatedDialog>
    {
        private const string NotMatchingActorsPattern =
            @"Not matching actors on performance step with index = {0}. Expected actor: (.*), actual actor: (.*).";

        private const string PerformanceStepNotCoveredPattern =
                "Error while testing a story: dialog produced a step #{0} from (.*) with message '(.*)' which was not covered by test story."
            ;

        private const string StoryStepNotCoveredPattern =
                "Error while testing a story: test story produced a step #{0} from (.*) with message '(.*)' which was not covered by performed story."
            ;

        private const string NotEqualDialogStatusMessagePattern =
            "Expected dialog status = '.*', actual status = '.*'";

        private const string ResultEmptyMessage = "Couldn't check result predicate - result is null.";

        [Fact]
        public async Task StoryWithWrongInitialActor_PlayStoryIsCalled_TrueExceptionThrown()
        {
            var story = StoryRecorder
                .Record()
                .User.Says("Hi")
                .Bot.Says("Hi")
                .Bot.Says("How are you?")
                .Rewind();

            var notMatchingActorsPattern = string.Format(CultureInfo.InvariantCulture, NotMatchingActorsPattern, 0);

            await this.ThrowsTrueException(story, notMatchingActorsPattern);
        }

        [Fact]
        public async Task StoryWithNotCoveredPerformanceStep_PlayStoryIsCalled_TrueExceptionThrown()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Hi")
                .Rewind();

            var stepNotCoveredPattern = string.Format(CultureInfo.InvariantCulture, PerformanceStepNotCoveredPattern, 2);

            await this.ThrowsTrueException(story, stepNotCoveredPattern);
        }

        [Fact]
        public async Task StoryWithNotCoveredStoryStep_PlayStoryIsCalled_TrueExceptionThrown()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Hi")
                .Bot.Says("How are you?")
                .Bot.Says("How can I help you?")
                .Rewind();

            var stepNotCoveredPattern = string.Format(CultureInfo.InvariantCulture, StoryStepNotCoveredPattern, 3);

            await this.ThrowsTrueException(story, stepNotCoveredPattern);
        }

        [Fact]
        public async Task PartialStoryWithDialogDoneCheck_PlayStoryIsCalled_AssertTrueThrown()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Hi")
                .Bot.Says("How are you?")
                .DialogDone();

            await this.ThrowsTrueException(story, NotEqualDialogStatusMessagePattern);
        }

        [Fact]
        public async Task TestWithResultPredicate_PlayStoryIsCalled_AssertTrueThrown()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Hi")
                .User.Says("Hi")
                .Bot.Says("How are you?")
                .User.Says("I'm good.")
                .DialogDoneWithResult(result => (bool)result == true);

            await this.ThrowsTrueException(story, ResultEmptyMessage);
        }
    }
}
