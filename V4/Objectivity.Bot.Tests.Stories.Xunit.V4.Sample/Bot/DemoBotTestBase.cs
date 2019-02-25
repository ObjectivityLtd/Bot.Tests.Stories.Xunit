namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Sample.Bot
{
    using DemoBot;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class DemoBotTestBase : BotTestBase<DemoBot>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            DemoConfiguration.RegisterServices(services);
        }
    }
}