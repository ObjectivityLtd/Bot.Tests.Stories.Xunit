namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Dialog
{
    using System.Threading.Tasks;
    using DemoBot.Dialogs;
    using DemoBot.User;
    using Extensions;
    using global::Xunit;
    using Microsoft.Bot.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class TestChoicePromptDialogTests : DialogTestBase<TestChoicePromptDialog>
    {
        [Fact]
        public async Task ChoicePrompt_SelectedValidOption()
        {
            var story = this.Record
                .Bot.GivesChoice("Please choose your favorite color.", new[] { "Red", "Green", "Blue" })
                .User.Says("Red")
                .Bot.Says("Please choose your favorite fruit. (1) Apple, (2) Banana, or (3) Orange")
                .User.Says("Orange")
                .Bot.Says("Your favorite fruit is Orange and color is Red")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task ChoicePrompt_SelectedInvalidColor_ValidationMessageIsReturned()
        {
            var story = this.Record
                .Bot.GivesChoice("Please choose your favorite color.", new[] { "Red", "Green", "Blue" })
                .User.Says("Orange")
                .Bot.GivesChoice("Sorry, please choose a color from the list.", new[] { "Red", "Green", "Blue" })
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task ChoicePrompt_SelectedInvalidFruit_ValidationMessageIsReturned()
        {
            var story = this.Record
                .Bot.GivesChoice("Please choose your favorite color.", new[] { "Red", "Green", "Blue" })
                .User.Says("Red")
                .Bot.Says("Please choose your favorite fruit. (1) Apple, (2) Banana, or (3) Orange")
                .User.Says("Red")
                .Bot.Says("Sorry, please choose a fruit from the list. (1) Apple, (2) Banana, or (3) Orange")
                .Rewind();

            await this.Play(story);
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddScoped(sp => new DemoUserStateAccessors(new UserState(this.DataStore)));
        }
    }
}
