namespace Objectivity.Bot.Tests.Stories.Xunit.Sample
{
    using System;
    using System.Threading.Tasks;
    using Asserts;
    using Dialogs;
    using global::Xunit;
    using Recorder;

    public class SumDialogTests : DialogUnitTestBase<SumDialog>
    {
        private const string WrongExceptionTypeMessagePattern = "Expected dialog fail with exception of type = '.*', actual exception type = '.*'";
        private const string NotEqualDialogResultMessagePattern = "Dialog result = '.*' doesn't match test predicate.";
        private const string WrongDialogResultTypeMessagePattern = "Dialog result = '.*' is not of an expected type.";

        [Fact]
        public async Task Sum40With60_PlayStoryIsCalled_DialogResultIsEqualTo100()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("60")
                .DialogDoneWithResult(result => (int)result == 100);

            await this.Play(story);
        }

        [Fact]
        public async Task Sum40With60_PlayStoryIsCalled_DialogResultIsGreaterThanOrEqualTo100()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("60")
                .DialogDoneWithResult(result => (int)result >= 100);

            await this.Play(story);
        }

        [Fact]
        public async Task Sum40With60_PlayStoryIsCalled_IntDialogResultIsEqualTo100()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("60")
                .DialogDoneWithResult<int>(result => result >= 100);

            await this.Play(story);
        }

        [Fact]
        public async Task ExpectStringDialogResult_PlayStoryIsCalled_FormatExceptionThrown()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("60")
                .DialogDoneWithResult<string>(string.IsNullOrEmpty);

            await Assert.ThrowsAsync<FormatException>(async () =>
            {
                await this.Play(story);
            });
        }

        [Fact]
        public async Task Sum40WithNaN_PlayStoryIsCalled_DialogFailedExpected()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("NaN")
                .DialogFailed();

            await this.Play(story);
        }

        [Fact]
        public async Task Sum40WithNaN_PlayStoryIsCalled_DialogFailedWithFormatExceptionExpected()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("NaN")
                .DialogFailedWithExceptionOfType<FormatException>();

            await this.Play(story);
        }

        [Fact]
        public async Task DialogFailedWithWronglyExpectedExceptionType_PlayStoryIsCalled_TrueExceptionExpected()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("40")
                .Bot.Says("Type second number:")
                .User.Says("NaN")
                .DialogFailedWithExceptionOfType<ArgumentNullException>();

            await this.ThrowsTrueException(story, WrongExceptionTypeMessagePattern);
        }

        [Fact]
        public async Task WrongResultExpected_PlayStoryIsCalled_TrueExceptionExpected()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("2")
                .Bot.Says("Type second number:")
                .User.Says("2")
                .DialogDoneWithResult(result => (int)result == 5);

            await this.ThrowsTrueException(story, NotEqualDialogResultMessagePattern);
        }

        [Fact]
        public async Task WrongTypeOfResultExpected_PlayStoryIsCalled_TrueExceptionExpected()
        {
            var story = StoryRecorder
                .Record()
                .Bot.Says("Type first number:")
                .User.Says("2")
                .Bot.Says("Type second number:")
                .User.Says("2")
                .DialogDoneWithResult(result => (string)result == string.Empty);

            await this.ThrowsTrueException(story, WrongDialogResultTypeMessagePattern);
        }
    }
}
