namespace Objectivity.Bot.Tests.Stories.Xunit.V4.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dialogs;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;

    internal class DummyDialogBot : IBot
    {
        private readonly DialogSet dialogs;
        private readonly ConversationState conversationState;

        public DummyDialogBot(
            Dialog dialog,
            WrappedDialogResult result,
            ConversationState conversationState)
        {
            this.conversationState = conversationState;

            this.dialogs = new DialogSet(conversationState.CreateProperty<DialogState>("dummyDialogStateAccessor"));
            this.dialogs.Add(new DialogProxy(dialog, result));
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dc = await this.dialogs.CreateContextAsync(turnContext, cancellationToken);
            var results = await dc.ContinueDialogAsync(cancellationToken);

            if (!dc.Context.Responded && results.Status == DialogTurnStatus.Empty && dc.ActiveDialog == null)
            {
                await dc.BeginDialogAsync(nameof(DialogProxy), null, cancellationToken);
            }

            await this.conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}