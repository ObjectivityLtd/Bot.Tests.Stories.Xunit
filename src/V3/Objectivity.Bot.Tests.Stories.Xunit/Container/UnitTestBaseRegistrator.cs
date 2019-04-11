namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using System;
    using Autofac;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Stories.Core;

    public class UnitTestBaseRegistrator
    {
        public static void RegisterTestComponents<TDialog>(ContainerBuilder containerBuilder, ChannelAccount from, Func<IComponentContext, TDialog> registerDialogFunc = null)
            where TDialog : IDialog<object>
        {
            var conversationService = new ConversationService(from);

            containerBuilder
                .Register(c => conversationService)
                .As<IConversationService<IMessageActivity>>()
                .SingleInstance();

            if (registerDialogFunc == null)
            {
                containerBuilder
                    .RegisterType<TDialog>()
                    .Keyed<IDialog<object>>(Constants.TargetDialogKey)
                    .InstancePerDependency();
            }
            else
            {
                containerBuilder.Register(registerDialogFunc)
                    .Keyed<IDialog<object>>(Constants.TargetDialogKey)
                    .InstancePerDependency();
            }
        }
    }
}
