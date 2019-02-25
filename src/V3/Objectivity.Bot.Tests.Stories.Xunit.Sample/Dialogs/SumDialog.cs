namespace Objectivity.Bot.Tests.Stories.Xunit.Sample.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class SumDialog : IDialog<object>
    {
        private int firstNumber;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Type first number:");

            context.Wait(this.ResumeAfterFirstResponse);
        }

        private async Task ResumeAfterFirstResponse(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var response = await item;

            try
            {
                this.firstNumber = int.Parse(response.Text);

                await context.PostAsync("Type second number:");
                context.Wait(this.ResumeAfterLastResponse);
            }
            catch (Exception ex)
            {
                context.Fail(ex);
            }
        }

        private async Task ResumeAfterLastResponse(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var response = await item;

            try
            {
                var secondNumber = int.Parse(response.Text);
                var result = this.firstNumber + secondNumber;
                context.Done<object>(result);
            }
            catch (Exception ex)
            {
                context.Fail(ex);
            }
        }
    }
}
