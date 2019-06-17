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

### Testing members joined conversation

You can use specialized method for setting conversation update member. 

```cs
public class UserWelcomeMessageTests: BotTestBase<DemoBot>
{
    [Fact]
    public async Task UserJoinedConversation_PlayStoryIsCalled_MustShowWelcomeMessage()
    {
        var story = this.Record
            .Configuration.SetConversationUpdateMembers(ChannelId.User)
            .Bot.Says("Welcome to demo bot")
            .Rewind();

        await this.Play(story);
    }
}
```

### Testing with mocks

You can use specialized method for setting channel. 

```cs
public class ServiceMocksTests: DemoBotTestBase
{
    [Fact]
    public async Task ConversationInitiatedWithServiceMock_PlayStoryIsCalled_MustReturnMockedResults()
    {
        var expectedResult = 0;
        var roomServiceMock = new Mock<IRoomService>();
        roomServiceMock.Setup(x => x.GetRoomFloorByNumber(It.IsAny<decimal>())).Returns(expectedResult);

        var story = this.Record
            .Configuration.RegisterService(services => services.AddScoped(sp => roomServiceMock.Object))
            .Bot.Says("Welcome to demo bot")
            .User.Says("room test")
            .Bot.Says("What's the room number?")
            .User.Says("1.55")
            .Bot.Says($"Room floor is: {expectedResult}")
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

### Bot with events

You can use specialized method for bot with events:

```cs
public class EventTests : BotTestBase<DemoBot>
{
    [Fact]
    public async Task EventAuthentication_BotSendsRequestsTokenEvent()
    {
        var story = this.Record
            .Bot.SendsEvent("tokens/request")
            .Rewind();

        await this.Play(story);
    }
}
```

### Middleware

You can register middleware that will be executed on each turn:

```cs
public class MiddlewareTests : BotTestBase<DemoBot>
{
    [Fact]
    public async Task GreetingMiddlewareRegistered_BotResponsWithGreetingMessage()
    {
        var story = this.Record
            .Configuration.RegisterService(
                services => services.AddScoped<IMiddleware>(factory => new GreetingMiddleware()))
            .Bot.Says("Welcome from Greeting middleware")
            .Rewind();

        await this.Play(story);
    }
}
```

> **IMPORTANT:** Middleware must be registered as IMiddleware interface.

### Bot adapter configuration
You can mock user access tokens by using `BotAdapterConfiguration` property in `BotTestBase<T>` base class:

```cs
public class AccessTokenTests : BotTestBase<DemoBot>
{
    [Fact]
    public async Task UserHasToken_PlayStoryIsCalled_BotFoundToken()
    {
        const string channelId = "testChannel";

        this.BotAdapterConfiguration.UserAccessTokens.Add(new UserAccessTokenConfiguration
        {
            UserId = ChannelId.User,
            ChannelId = channelId,
        });

        var story = this.Record
            .Configuration.UseChannel(channelId)
            .User.Says("Who am I")
            .Bot.Says($"You are {ChannelId.User}")
            .Rewind();

        await this.Play(story);
    }
}
```

### Custom user activity

You can use specialized method for sending any activity to the bot:

```cs
public class EventTests : BotTestBase<DemoBot>
{
    [Fact]
    public async Task EventAuthentication_UserSendsTokenResponseEvent()
    {
        var conversationId = Guid.NewGuid().ToString();
        var story = this.Record
            .Configuration.WithConversationId(conversationId)
            .User.SendsActivity(
                new Activity
                {
                    Type = ActivityTypes.Event,
                    Name = "tokens/response",
                    ChannelId = "Test",
                    Conversation = new ConversationAccount { Id = conversationId },
                    From = new ChannelAccount { Id = ChannelId.User },
                    Recipient = new ChannelAccount { Id = ChannelId.Bot },
                })
            .Rewind();

        await this.Play(story);
    }
}
```

> **IMPORTANT:** To keep context in conversation flow you should keep the same `conversationId` for each activity that is sent to the dialog. See how this is achieved by `.Configuration.WithConversationId(conversationId)` in example above.

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
