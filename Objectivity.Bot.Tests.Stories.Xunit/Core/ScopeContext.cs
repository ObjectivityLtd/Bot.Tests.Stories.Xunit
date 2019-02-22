namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using Autofac;
    using Microsoft.Bot.Builder.Dialogs.Internals;
    using Microsoft.Bot.Connector;
    using Stories.Core;

    public class ScopeContext : IScopeContext
    {
        private bool disposed;

        public ScopeContext(IContainer container)
        {
            var conversationService = container.Resolve<IConversationService<IMessageActivity>>();
            var messageActivity = conversationService.GetToBotActivity(string.Empty);

            this.Scope = DialogModule.BeginLifetimeScope(container, messageActivity);
        }

        public ILifetimeScope Scope { get; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Scope.Dispose();
            }

            this.disposed = true;
        }
    }
}
