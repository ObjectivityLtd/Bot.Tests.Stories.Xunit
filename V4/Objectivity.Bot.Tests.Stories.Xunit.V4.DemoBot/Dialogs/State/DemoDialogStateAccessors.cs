namespace Objectivity.Bot.Tests.Stories.Xunit.V4.DemoBot.Dialogs.State
{
    using Microsoft.Bot.Builder;
    using System;
    using Microsoft.Bot.Builder.Dialogs;

    public class DemoDialogStateAccessors
    {
        public DemoDialogStateAccessors(ConversationState conversationState)
        {
            this.ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));

            this.DialogStateAccessor = conversationState.CreateProperty<DialogState>(DialogStateAccessorKey);
            this.DemoDialogStateAccessor =
                conversationState.CreateProperty<DemoDialogState>(DemoDialogStateAccessorKey);
        }

        public static string DialogStateAccessorKey { get; } = $"{nameof(DemoDialogStateAccessors)}.{nameof(DialogStateAccessor)}";
        public static string DemoDialogStateAccessorKey { get; } = $"{nameof(DemoDialogStateAccessors)}.{nameof(DemoDialogStateAccessor)}";

        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }
        public IStatePropertyAccessor<DemoDialogState> DemoDialogStateAccessor { get; set; }

        public ConversationState ConversationState { get; }
    }
}
