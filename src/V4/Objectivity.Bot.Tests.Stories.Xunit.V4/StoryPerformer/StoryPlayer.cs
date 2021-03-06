﻿namespace Objectivity.Bot.Tests.Stories.Xunit.V4.StoryPerformer
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Asserts;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.DependencyInjection;
    using Player;
    using StoryModel;

    public class StoryPlayer : IStoryPlayer<IMessageActivity>
    {
        private readonly IServiceProvider serviceProvider;

        public StoryPlayer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Play(IStory<IMessageActivity> story, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var container = scope.ServiceProvider;
                var storyPerformer = container.GetService<IStoryPerformer<IMessageActivity>>();
                var performanceSteps = await storyPerformer.Perform(story);
                var storyAsserts = container.GetService<StoryAsserts>();

                await storyAsserts.AssertStory(story, performanceSteps.Where(s => s.MessageActivity.Type != ActivityTypes.Trace).ToList());
            }
        }
    }
}
