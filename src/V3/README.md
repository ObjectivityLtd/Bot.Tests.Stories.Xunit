# Bot.Tests.Stories.Xunit

[![Build Status](https://ci.appveyor.com/api/projects/status/github/ObjectivityLtd/Bot.Tests.Stories.Xunit?branch=master&svg=true)](https://ci.appveyor.com/project/ObjectivityAdminsTeam/bot-tests-stories-xunit) [![Tests Status](https://img.shields.io/appveyor/tests/ObjectivityAdminsTeam/bot-tests-stories-xunit/master.svg)](https://ci.appveyor.com/project/ObjectivityAdminsTeam/bot-tests-stories-xunit) [![codecov](https://codecov.io/gh/ObjectivityLtd/Bot.Tests.Stories.Xunit/branch/master/graph/badge.svg)](https://codecov.io/gh/ObjectivityLtd/Bot.Tests.Stories.Xunit) [![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://opensource.org/licenses/MIT)

Tests library for [Microsoft Bot Framework V3](https://dev.botframework.com/) dialogs using [XUnit](https://xunit.github.io/).

## Note

This project is still a work in progress, all contributions from your side are desirable!

## Installation

You can install the package using the nuget:

```powershell
Install-Package Objectivity.Bot.Tests.Stories.Xunit
```

## Limitations

At the moment test base classes are permitted only for dialogs returning object type. Story recorder used in this library does not allow to recognize user identity during conversation test.

## Usage

## Simple dialogs

To develop a unit test for a dialog, create new test class inheriting from `Objectivity.Bot.Tests.Stories.Xunit.DialogUnitTestBase<T>` class, providing your dialog Type as generic parameter. Then for each test please go through the following steps:

* Record a story
* Rewind
* Play (assert)

Example:

```cs
public class EchoDialogTests : DialogUnitTestBase<EchoDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .User.Says("Hello")
            .Bot.Says("You said Hello")
            .Rewind();

        await this.Play(story);
    }
}
```

### Dialog with prompts

You can use specialized method for dialogs with prompts:

```cs
public class PropmtDialogTests : DialogUnitTestBase<PropmtDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .Bot.Confirms("Are you enjoying this conversation?")
            .User.Says("Yes")
            .Bot.GivesChoice(
                "Who is a better conversationalist?",
                new[] { "Bot", "Real person", "Rubber duck" })
            .User.Says("Bot")
            .Bot.Says("Thank you")
            .Rewind();

        await this.Play(story);
    }
}
```

### Dialog finish scenarios

You can also define various dialog finish scenarios.

#### Dialog Done

The example below assumes the Dialog calls `context.Done()` after bot posts first reply.

```cs
public class MyDialogTests : DialogUnitTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .User.Says("Hi")
            .Bot.Says("Good bye")
            .DialogDone();

        await this.Play(story);
    }
}
```

#### Dialog Done with result predicate

The example below assumes the Dialog calls `context.Done(true)` after bot posts first reply.

```cs
public class MyDialogTests : DialogUnitTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .User.Says("Hi")
            .Bot.Says("Good bye")
            .DialogDoneWithResult<bool>(result => result == true);

        await this.Play(story);
    }
}
```

#### Dialog Failed

The example below assumes the Dialog calls `context.Fail(ex)` with any kind of exception after first user sentence.

```cs
public class MyDialogTests : DialogUnitTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .Bot.Says("Type a number:")
            .User.Says("Ok")
            .DialogFailed();

        await this.Play(story);
    }
}
```

#### Dialog Failed with expected exception type

The example below assumes the Dialog calls `context.Fail(ex)` with specific kind of exception after first user sentence.

```cs
public class MyDialogTests : DialogUnitTestBase<MyDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .Bot.Says("Type a number:")
            .User.Says("Ok")
            .DialogFailedWithExceptionOfType<FormatException>();

        await this.Play(story);
    }
}
```

### LUIS dialogs

To develop a unit test for a LUIS dialog (inheriting `LuisDialog<object>` class), create new test class inheriting from `Objectivity.Bot.Tests.Stories.Xunit.LuisDialogUnitTestBase<T>` class, providing your dialog Type as generic parameter. Then for each test please go through the following steps:

* Register utterance (for intent test)
* Record a story
* Rewind
* Play (assert)

Example:

```cs
public class EchoDialogTests : LuisDialogUnitTestBase<EchoDialog>
{
    [Fact]
    public async Task HelloTest()
    {
        var story = StoryRecorder
            .Record()
            .User.Says("Hello")
            .Bot.Says("You said Hello")
            .Rewind();

        await this.Play(story);
    }
}
```

### Injecting dependencies

If your dialog requires some dependencies injected using Autofac, you can provide them by overloading `RegisterAdditionalTypes` protected method. Example:

```cs
protected override void RegisterAdditionalTypes(ContainerBuilder builder)
{
    builder.RegisterType<EchoService>().As<IEchoService>();
}
```

If you need to register the dialog in a customized way, you can use RegisterDialog method from base class. Example:
```cs
this.RegisterDialog((x) =>
{
    var dialog = new WithInitDialog();
    dialog.Init();
    return dialog;
});
```