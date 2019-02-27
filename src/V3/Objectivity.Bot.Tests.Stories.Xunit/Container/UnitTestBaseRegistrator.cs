namespace Objectivity.Bot.Tests.Stories.Xunit.Container
{
    using Autofac;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Stories.Core;

    public class UnitTestBaseRegistrator
    {
        public static void RegisterTestComponents<TDialog>(ContainerBuilder containerBuilder, ChannelAccount from)
            where TDialog : IDialog<object>
        {
            var conversationService = new ConversationService(from);

            containerBuilder
                .Register(c => conversationService)
                .As<IConversationService<IMessageActivity>>()
                .SingleInstance();

            containerBuilder
                .RegisterType<TDialog>()
                .Keyed<IDialog<object>>(Constants.TargetDialogKey)
                .InstancePerDependency();
        }
    }
}
