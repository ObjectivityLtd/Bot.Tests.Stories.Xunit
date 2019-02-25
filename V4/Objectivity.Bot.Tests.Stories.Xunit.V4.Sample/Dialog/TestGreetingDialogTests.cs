namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using DemoBot.Dialogs.State;
    using DemoBot.User;
    using global::Xunit;
    using Microsoft.Bot.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class TestGreetingDialogTests: DialogTestBase<TestGreetingDialog>
    {
        [Fact]
        public async Task TwoFloorsPassed_PlayStoryIsCalled_MustShowMotivationMessage()
        {
            var story = this.Record
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How many floors have you climbed today?")
                .User.Says("2")
                .Bot.Says("Come back when it's at least 3 John.")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task ThreeFloorsPassed_PlayStoryIsCalled_MustShowMotivationMessage()
        {
            var story = this.Record
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How many floors have you climbed today?")
                .User.Says("3")
                .Bot.Says("Great score!")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task InvalidNumberPassed_PlayStoryIsCalled_MustRepromtForNumberOfFloors()
        {
            var story = this.Record
                .Bot.Says("What's your name?")
                .User.Says("John")
                .Bot.Says("How many floors have you climbed today?")
                .User.Says("no")
                .Bot.Says("How many floors have you climbed today?")
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
