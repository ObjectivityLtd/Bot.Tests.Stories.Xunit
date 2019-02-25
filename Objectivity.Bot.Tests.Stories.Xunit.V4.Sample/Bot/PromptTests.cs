﻿namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using System.Threading.Tasks;
    using Extensions;
    using global::Xunit;

    public class PromptTests : DemoBotTestBase
    {
        [Fact]
        public async Task ChoicePrompt_SelectedValidOption()
        {
            var story = this.Record
                .Bot.Says("Welcome to demo bot")
                .User.Says("choice test")
                .Bot.GivesChoice("Please choose your favorite color.", new[] { "Red", "Green", "Blue" })
                .User.Says("Red")
                .Bot.Says("Your favorite color is Red")
                .Rewind();

            await this.Play(story);
        }

        [Fact]
        public async Task ChoicePrompt_SelectedInvalidOption_ValidationMessageIsReturned()
        {
            var story = this.Record
                .Bot.Says("Welcome to demo bot")
                .User.Says("choice test")
                .Bot.GivesChoice("Please choose your favorite color.", new [] { "Red", "Green", "Blue" })
                .User.Says("Orange")
                .Bot.GivesChoice("Sorry, please choose a color from the list.", new[] { "Red", "Green", "Blue" })
                .Rewind();

            await this.Play(story);
        }
    }
}