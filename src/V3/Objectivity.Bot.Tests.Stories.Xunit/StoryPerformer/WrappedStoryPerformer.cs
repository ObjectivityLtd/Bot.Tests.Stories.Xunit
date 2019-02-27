namespace Objectivity.Bot.Tests.Stories.Xunit.StoryPerformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using IO;
    using Microsoft.Bot.Connector;
    using Player;
    using Recorder;
    using Stories.Core;
    using Stories.Extensions;
    using StoryModel;

    public class WrappedStoryPerformer : IStoryPerformer<IMessageActivity>
    {
        private readonly IConversationService<IMessageActivity> conversationService;
        private readonly IDialogReader<IMessageActivity> dialogReader;
        private readonly IDialogWriter<IMessageActivity> dialogWriter;
        private readonly IPerformanceStory<IMessageActivity> performanceStory;
        private readonly WrappedDialogResult wrappedDialogResult;

        public WrappedStoryPerformer(
            IScopeContext scopeContext,
            IConversationService<IMessageActivity> conversationService,
            WrappedDialogResult wrappedDialogResult)
        {
            this.conversationService = conversationService;

            this.wrappedDialogResult = wrappedDialogResult;

            this.performanceStory = new PerformanceStory();
            this.dialogReader = new WrapperDialogReader(scopeContext);
            this.dialogWriter = new WrappedDialogWriter(scopeContext, conversationService);
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
            var wrapperStory = StoryRecorder.Record()
                .User.Says(Constants.WrapperStartMessage)
                .Bot.Says(Constants.WrapperStartMessage)
                .Rewind();

            var wrappedStory = testStory.Concat(wrapperStory);

            return wrappedStory.StoryFrames.Select((storyFrame, stepIndex) =>
                new StoryStep<IMessageActivity>(storyFrame, isDialogResultCheckupStep: storyFrame is DialogStoryFrame)
                {
                    Status = StoryPlayerStepStatus.NotDone,
                    StepIndex = stepIndex
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
