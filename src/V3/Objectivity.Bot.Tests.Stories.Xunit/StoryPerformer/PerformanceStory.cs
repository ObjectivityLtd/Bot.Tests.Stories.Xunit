﻿namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer
{
    using Microsoft.Bot.Connector;
    using Stories.StoryPerformer;
    using StoryModel;

    public class PerformanceStory : PerformanceStory<IMessageActivity>
    {
        private const string TypingMessageType = "typing";

        protected override PerformanceStep<IMessageActivity> GetPerformanceStep(IMessageActivity message, Actor actor)
        {
            if (message.Type == TypingMessageType || message.Text == Consts.WrapperStartMessage)
            {
                return null;
            }

            var performanceStep = new PerformanceStep(message)
            {
                Actor = actor
            };

            return performanceStep;
        }
    }
}