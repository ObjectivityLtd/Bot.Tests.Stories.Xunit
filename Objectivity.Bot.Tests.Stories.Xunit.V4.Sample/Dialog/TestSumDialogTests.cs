namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using DemoBot.Dialogs.State;
    using DemoBot.User;
    using Extensions;
    using global::Xunit;
    using Microsoft.Bot.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class TestSumDialogTests: DialogTestBase<TestSumDialog>
    {
        [Fact]
        public async Task CorrectNumbersPassed_PlayStoryIsCalled_MustReturnSum()
        {
            var story = this.Record
                .Bot.Says("What's the first number?")
                .User.Says("5")
                .Bot.Says("What's the second number?")
                .User.Says("12")
                .Bot.Says("Sum is: 17")
                .DialogDoneWithResult<int>(x => x == 17);

            await this.Play(story);
        }

        [Fact]
        public async Task NegativeValuePassed_PlayStoryIsCalled_MustRepromptForNumber()
        {
            var story = this.Record
                .Bot.Says("What's the first number?")
                .User.Says("-5")
                .Bot.Says("A number must be between 0 and 100. Please try again.")
                .User.Says("5")
                .Bot.Says("What's the second number?")
                .User.Says("-12")
                .Bot.Says("A number must be between 0 and 100. Please try again.")
                .User.Says("12")
                .Bot.Says("Sum is: 17")
                .DialogDoneWithResult<int>(x => x == 17);

            await this.Play(story);
        }

        [Fact]
        public async Task ValueGreaterThan100Passed_PlayStoryIsCalled_MustRepromptForNumber()
        {
            var story = this.Record
                .Bot.Says("What's the first number?")
                .User.Says("105")
                .Bot.Says("A number must be between 0 and 100. Please try again.")
                .User.Says("5")
                .Bot.Says("What's the second number?")
                .User.Says("112")
                .Bot.Says("A number must be between 0 and 100. Please try again.")
                .User.Says("12")
                .Bot.Says("Sum is: 17")
                .DialogDoneWithResult<int>(x => x == 17);

            await this.Play(story);
        }

        [Fact]
        public async Task NonNumericValuePassed_PlayStoryIsCalled_MustRepromptForNumber()
        {
            var story = this.Record
                .Bot.Says("What's the first number?")
                .User.Says("NaN")
                .Bot.Says("A number must be between 0 and 100. Please try again.")
                .Rewind();

            await this.Play(story);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddScoped(sp => new DemoUserStateAccessors(new UserState(this.DataStore)));
            services.AddScoped(sp => new DemoDialogStateAccessors(sp.GetService<ConversationState>()));
        }
    }
}
