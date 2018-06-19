namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPlayer
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Asserts;
    using Autofac;
    using Container;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Connector;
    using Player;
    using StoryModel;
    using StoryPerformer;

    public class UnitTestStoryPlayer : IStoryPlayer
    {
        private readonly ITestContainerBuilder testContainerBuilder;

        public UnitTestStoryPlayer(ITestContainerBuilder testContainerBuilder)
        {
            this.testContainerBuilder = testContainerBuilder;
        }

        public async Task<IStoryResult> Play(IStory story, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var container = this.GetDialogTestContainer())
            using (var scopeContext = InitializeScopeContext(container))
            {
                var testDialog = container.Resolve<IDialog<object>>();
                var storyPerformer = container.Resolve<IStoryPerformer>();

                DialogModule_MakeRoot.Register(scopeContext.Scope, () => testDialog);

                var performanceSteps = await storyPerformer.Perform(story);

                var storyAsserts = container.Resolve<StoryAsserts>();

                await storyAsserts.AssertStory(story, performanceSteps.Where(s => s.MessageActivity.Type != ActivityTypes.Trace).ToList());
            }

            return new StoryResult();
        }

        private static IScopeContext InitializeScopeContext(IContainer container)
        {
            var scopeContext = new ScopeContext(container);
            var builder = new ContainerBuilder();
            var module = new UnitTestStoryPlayerModule(scopeContext);

            builder.RegisterModule(module);
            builder.Update(container);

            return scopeContext;
        }

        private IContainer GetDialogTestContainer()
        {
            using (new ResolveMoqAssembly())
            {
                var options = TestContainerBuilderOptions.MockConnectorFactory
                    | TestContainerBuilderOptions.ScopedQueue
                    | TestContainerBuilderOptions.ResolveDialogFromContainer;

                return this.testContainerBuilder.Build(options);
            }
        }
    }
}
