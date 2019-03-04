# Bot.Tests.Stories.Xunit.V4

[![Build Status](https://ci.appveyor.com/api/projects/status/github/ObjectivityLtd/Bot.Tests.Stories.Xunit?branch=master&svg=true)](https://ci.appveyor.com/project/ObjectivityAdminsTeam/bot-tests-stories-xunit) [![Tests Status](https://img.shields.io/appveyor/tests/ObjectivityAdminsTeam/bot-tests-stories-xunit/master.svg)](https://ci.appveyor.com/project/ObjectivityAdminsTeam/bot-tests-stories-xunit) [![codecov](https://codecov.io/gh/ObjectivityLtd/Bot.Tests.Stories.Xunit/branch/master/graph/badge.svg)](https://codecov.io/gh/ObjectivityLtd/Bot.Tests.Stories.Xunit) [![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)

Tests library for [Microsoft Bot Framework V4](https://dev.botframework.com/) using [XUnit](https://xunit.github.io/).

## Note

This project is still a work in progress, all contributions from your side are desirable!

## Installation

You can install the package using the nuget:

```powershell
Install-Package Objectivity.Bot.Tests.Stories.Xunit.V4
```

## Limitations

Story recorder used in this library does not allow to recognize user identity during conversation test.

# Usage

## Testing Bot

To develop a tests for bot, create new test class inheriting from `Objectivity.Bot.Tests.Stories.Xunit.V4.BotTestBase<T>` class, providing your bot type as generic parameter. Then for each test please go through the following steps:

* Record a story
* Rewind
* Play (assert)

Example:

### Testing welcome message

Example below assumes that bot sends welcome message on `ConversationUpdate` activity. 

```cs
public class WelcomeMessageTests: BotTestBase<DemoBot>
{
    [Fact]
    public async Task ConversationInitiated_PlayStoryIsCalled_MustShowWelcomeMessage()
    {
        var story = this.Record
            .Bot.Says("Welcome to demo bot")
            .Rewind();

        await this.Play(story);
    }
}
```

### Testing channels

You can use specialized method for setting channel. 

```cs
public class ChannelConfigurationTests: BotTestBase<DemoBot>
{
    [Fact]
    public async Task ConversationInitiatedWithFacebookChannel_PlayStoryIsCalled_MustShowFacebookWelcomeMessage()
    {
        var story = this.Record
            .Configuration.UseChannel(Channels.Facebook)
            .Bot.Says("Welcome to demo bot facebook")
            .Rewind();

        await this.Play(story);
    }
}
```

### Bot with prompts

You can use specialized method for dialogs with prompts:

```cs
public class PromptTests : BotTestBase<DemoBot>
{
    [Fact]
    public async Task ChoicePrompt_SelectedValidOption()
    {
        var story = this.Record
            .User.Says("Let me tell you about color")
            .Bot.GivesChoice("Please choose your favorite color.", new[] { "Red", "Green", "Blue" })
            .User.Says("Red")
            .Bot.Says("Your favorite color is Red")
            .Rewind();

        await this.Play(story);
    }
}
```

## Testing dialogs

To develop a tests for a dialog, create new test class inheriting from `Objectivity.Bot.Tests.Stories.Xunit.V4.DialogTestBase<T>` class, providing your dialog type as generic parameter. Then for each test please go through the following steps:

* Record a story
* Rewind
* Play (assert)

Example:

```cs
public class TestGreetingDialogTests: DialogTestBase<TestGreetingDialog>
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
            .Bot.Says("Welcome to demo bot")
            .User.Says("hello")
            .Bot.Says("What's your name?")
            .User.Says("John")
            .Bot.Says("How old are you?")
            .User.Says(age.ToString(CultureInfo.InvariantCulture))
            .Bot.Says(expectedBotReply)
            .Rewind();

        await this.Play(story);
    }
}
```

### Dialog finish scenarios

You can also define various dialog finish scenarios.

#### Dialog Done With Result

The example below assumes the Dialog call returns `DialogTurnStatus.Complete` status.

```cs
public class MyDialogTests : DialogTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = this.Record
            .User.Says("Hi")
            .Bot.Says("Good bye")
            .DialogDone();

        await this.Play(story);
    }
}
```

#### Dialog Done with result predicate

The example below assumes the Dialog call returns `DialogTurnStatus.Complete` status and expected result value.

```cs
public class SumDialogTests: DialogTestBase<SumDialog>
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
}
```

#### Dialog Failed

The example below assumes the Dialog call has thrown an exception.

```cs
public class MyDialogTests : DialogTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = this.Record
            .Bot.Says("Type a number:")
            .User.Says("Ok")
            .DialogFailed();

        await this.Play(story);
    }
}
```

#### Dialog Failed with expected exception type

The example below assumes the Dialog call has thrown an exception of a given type after first user sentence.

```cs
public class MyDialogTests : DialogTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = this.Record
            .Bot.Says("Type a number:")
            .User.Says("Ok")
            .DialogFailedWithExceptionOfType<FormatException>();

        await this.Play(story);
    }
}
```

## Injecting dependencies

If your test requires some dependencies injected, you can provide them by overloading `ConfigureServices` protected method. Example:

```cs
protected override void ConfigureServices(IServiceCollection services)
{
    base.ConfigureServices(services);

    services.AddScoped(sp => new DemoUserStateAccessors(new UserState(this.DataStore)));
    services.AddScoped(sp => new DemoDialogStateAccessors(sp.GetService<ConversationState>()));
}
```
