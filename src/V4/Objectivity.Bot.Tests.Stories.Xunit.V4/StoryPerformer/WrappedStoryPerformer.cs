namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Dialogs;
    using Microsoft.Bot.Schema;
    using Player;
    using Stories.Extensions;
    using StoryModel;

    public class WrappedStoryPerformer : IStoryPerformer<IMessageActivity>
    {
        private readonly IConversationService conversationService;
        private readonly IDialogReader<IMessageActivity> dialogReader;
        private readonly IDialogWriter<IMessageActivity> dialogWriter;
        private readonly IPerformanceStory<IMessageActivity> performanceStory;
        private readonly WrappedDialogResult wrappedDialogResult;

        public WrappedStoryPerformer(
            IDialogWriter<IMessageActivity> writer,
            IDialogReader<IMessageActivity> reader,
            IConversationService conversationService,
            WrappedDialogResult wrappedDialogResult)
        {
            this.conversationService = conversationService;

            this.wrappedDialogResult = wrappedDialogResult;

            this.performanceStory = new PerformanceStory();
            this.dialogReader = reader;
            this.dialogWriter = writer;
        }

        public async Task<List<PerformanceStep<IMessageActivity>>> Perform(IStory<IMessageActivity> testStory)
        {
            var steps = this.GetStorySteps(testStory);

            try
            {
                foreach (var step in steps)
                {
                    this.PushStartupMessageActivities();

                    await this.WriteUserMessageActivity(step);

                    this.ReadBotMessageActivities();

                    this.TrySetLatestOptions();
                }
            }
            catch (Exception ex)
            {
                this.wrappedDialogResult.DialogStatus = DialogStatus.Failed;
                this.wrappedDialogResult.Exception = ex;
            }

            return this.performanceStory.Steps;
        }

        private List<StoryStep<IMessageActivity>> GetStorySteps(IStory<IMessageActivity> testStory)
        {
            return testStory.StoryFrames.Select((storyFrame, stepIndex) =>
                new StoryStep<IMessageActivity>(storyFrame, isDialogResultCheckupStep: storyFrame is DialogStoryFrame<IMessageActivity>)
                {
                    Status = StoryPlayerStepStatus.NotDone,
                    StepIndex = stepIndex,
                })
                .ToList();
        }

        private void PushStartupMessageActivities()
        {
            this.performanceStory.PushStartupSteps();
        }

        private void ReadBotMessageActivities()
        {
            var startupMessageActivities = this.dialogReader.DequeueStartupMessageActivities();
            var messageActivities = this.dialogReader.GetMessageActivities();

            this.performanceStory.EnqueueStartupSteps(startupMessageActivities, Actor.Bot);
            this.performanceStory.AddSteps(messageActivities, Actor.Bot);
        }

        private void TrySetLatestOptions()
        {
            var options = this.performanceStory.Steps.TryGetOptions();
            this.conversationService.LatestOptions = options;
        }

        private async Task WriteUserMessageActivity(StoryStep<IMessageActivity> step)
        {
            if (step.Actor == Actor.User)
            {
                var messageActivity = this.dialogWriter.GetStepMessageActivity(step.StoryFrame);
                this.performanceStory.AddStep(messageActivity, Actor.User);

                await this.dialogWriter.SendActivity(messageActivity);
            }
        }
    }
}
