namespace Objectivity.Bot.Tests.Stories.Xunit.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Luis.Models;

    public class IntentUtterance<TDialog>
        where TDialog : IDialog<object>
    {
        public IntentUtterance()
        {
            this.Entities = new List<EntityRecommendation>();
        }

        public IntentUtterance(string utterance, Expression<Func<TDialog, Task>> intentAction)
        {
            this.Utterance = utterance;
            this.IntentAction = intentAction;
            this.Entities = new List<EntityRecommendation>();
        }

        public IntentUtterance(string utterance, Expression<Func<TDialog, Task>> intentAction, params EntityRecommendation[] entities)
        {
            this.Utterance = utterance;
            this.IntentAction = intentAction;
            this.Entities = entities.ToList();

            if (this.Entities.Any())
            {
                this.Score = 1.0;
            }
        }

        public string Utterance { get; set; }

        public double? Score { get; set; }

        public List<EntityRecommendation> Entities { get; set; }

        public Expression<Func<TDialog, Task>> IntentAction { get; set; }
    }
}