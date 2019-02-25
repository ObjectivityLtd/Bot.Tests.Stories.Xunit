namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using Autofac;

    public delegate void BeforeBuild(ContainerBuilder builder);

    public interface ITestContainerBuilder
    {
        IContainer Build(TestContainerBuilderOptions options, params object[] singletons);
    }
}
