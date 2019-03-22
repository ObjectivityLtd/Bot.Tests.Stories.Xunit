namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System.Globalization;
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using DemoBot.Dialogs.State;
    using DemoBot.User;
    using global::Xunit;
    using Microsoft.Bot.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class TestGreetingDialogTests : DialogTestBase<TestGreetingDialog>
    {
        [Theory]
        [InlineData(17, "I'm sorry John but you must be at least 18 years old.")]
        [InlineData(18, "Thank you.")]
        [InlineData(20, "Thank you.")]
        public async Task GivenAge_PlayStoryIsCalled_MustReplyExpectedMessage(
            int age,
            string expectedBotReply)
        {
            var story = this.Record
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How old are you?")
                .User.Says(age.ToString(CultureInfo.InvariantCulture))
                .Bot.Says(expectedBotReply)
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task InvalidNumberPassed_PlayStoryIsCalled_MustRepromptForAge()
        {
            var story = this.Record
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How old are you?")
                .User.Says("NaN")
                .Bot.Says("Your answer must be a number. Please try again.")
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
