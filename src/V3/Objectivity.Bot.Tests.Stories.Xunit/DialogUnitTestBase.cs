namespace Objectivity.Bot.Tests.Stories.Xunit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Container;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Player;
    using StoryModel;
    using StoryPlayer;

    public abstract class DialogUnitTestBase<TDialog> : DialogTestBase, IStoryPlayer<IMessageActivity>
        where TDialog : IDialog<object>
    {
        private Func<IComponentContext, TDialog> registerDialogInstanceFunc;

        protected ChannelAccount From { get; set; }

        public async Task Play(
            IStory<IMessageActivity> story,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var builder = this.GetTestContainerBuilder();
            var player = new UnitTestStoryPlayer(builder);

            await player.Play(story, cancellationToken);
        }

        protected virtual void RegisterAdditionalTypes(ContainerBuilder builder)
        {
        }

        protected void RegisterDialog(Func<IComponentContext, TDialog> regFunc)
        {
            this.registerDialogInstanceFunc = regFunc;
        }

        private ITestContainerBuilder GetTestContainerBuilder()
        {
            var builder = new TestContainerBuilder
            {
                AdditionalTypesRegistration = containerBuilder =>
                {
                    UnitTestBaseRegistrator.RegisterTestComponents(containerBuilder, this.From, this.registerDialogInstanceFunc);

                    this.RegisterAdditionalTypes(containerBuilder);
                },
            };

            return builder;
        }
    }
}
