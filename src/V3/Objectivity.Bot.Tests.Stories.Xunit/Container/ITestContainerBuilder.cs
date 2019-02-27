namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using Autofac;

    public interface ITestContainerBuilder
    {
        IContainer Build(TestContainerBuilderOptions options, params object[] singletons);
    }
}
