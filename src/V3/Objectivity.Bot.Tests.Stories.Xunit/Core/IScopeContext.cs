namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using Autofac;

    public interface IScopeContext : IDisposable
    {
        ILifetimeScope Scope { get; }
    }
}
