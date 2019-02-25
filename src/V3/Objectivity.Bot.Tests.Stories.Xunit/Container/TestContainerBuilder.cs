namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Connector;

    public class TestContainerBuilder : ITestContainerBuilder
    {
        public Action<ContainerBuilder> AdditionalTypesRegistration { get; set; }

        public IContainer Build(TestContainerBuilderOptions options, params object[] singletons)
        {
            var builder = new ContainerBuilder();
            if (options.HasFlag(TestContainerBuilderOptions.ResolveDialogFromContainer))
            {
                builder.RegisterModule(new DialogModule());
            }
            else
            {
                builder.RegisterModule(new DialogModule_MakeRoot());
            }

            // make a "singleton" MockConnectorFactory per unit test execution
            IConnectorClientFactory factory = null;
            builder
                .Register((c, p) => factory ?? (factory = new MockConnectorFactory(c.Resolve<IAddress>().BotId)))
                .As<IConnectorClientFactory>()
                .InstancePerLifetimeScope();

            if (options.HasFlag(TestContainerBuilderOptions.Reflection))
            {
                builder.RegisterModule(new ReflectionSurrogateModule());
            }

            var r =
                builder
                    .Register(c => new Queue<IMessageActivity>())
                    .AsSelf();

            if (options.HasFlag(TestContainerBuilderOptions.ScopedQueue))
            {
                r.InstancePerLifetimeScope();
            }
            else
            {
                r.SingleInstance();
            }

            builder
                .RegisterType<BotToUserQueue>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder
                .Register(c => new MapToChannelData_BotToUser(
                    c.Resolve<BotToUserQueue>(),
                    new List<IMessageActivityMapper> { new KeyboardCardMapper() }))
                .As<IBotToUser>()
                .InstancePerLifetimeScope();

            if (options.HasFlag(TestContainerBuilderOptions.LastWriteWinsCachingBotDataStore))
            {
                builder.Register(c => new CachingBotDataStore(c.ResolveKeyed<IBotDataStore<BotData>>(typeof(ConnectorStore)), CachingBotDataStoreConsistencyPolicy.LastWriteWins))
                    .As<IBotDataStore<BotData>>()
                    .AsSelf()
                    .InstancePerLifetimeScope();
            }

            if (singletons != null)
            {
                foreach (var singleton in singletons)
                {
                    builder
                        .Register(c => singleton)
                        .Keyed(FiberModule.Key_DoNotSerialize, singleton.GetType());
                }
            }

            this.AdditionalTypesRegistration?.Invoke(builder);

            return builder.Build();
        }
    }
}
