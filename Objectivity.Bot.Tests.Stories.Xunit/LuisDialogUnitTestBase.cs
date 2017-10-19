﻿namespace Objectivity.Bot.Tests.Stories.Xunit
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Container;
    using Core;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.Internals.Fibers;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Moq;
    using Player;
    using StoryModel;
    using StoryPlayer;

    public abstract class LuisDialogUnitTestBase<TDialog> : LuisTestBase, IStoryPlayer
        where TDialog : IDialog<object>
    {
        private readonly List<IntentUtterance<TDialog>> intentUtterances = new List<IntentUtterance<TDialog>>();

        public async Task<IStoryResult> Play(
            IStory story,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var builder = this.GetTestContainerBuilder();
            var player = new UnitTestStoryPlayer(builder);

            return await player.Play(story, cancellationToken);
        }

        protected virtual void RegisterAdditionalTypes(ContainerBuilder builder)
        {
        }

        protected void RegisterUtterance(string utterance, Expression<Func<TDialog, Task>> intentAction)
        {
            this.intentUtterances.Add(new IntentUtterance<TDialog>(utterance, intentAction));
        }

        protected void RegisterUtterance(string utterance, Expression<Func<TDialog, Task>> intentAction, params EntityRecommendation[] entities)
        {
            this.intentUtterances.Add(new IntentUtterance<TDialog>(utterance, intentAction, entities));
        }

        private ITestContainerBuilder GetTestContainerBuilder()
        {
            var builder = new TestContainerBuilder
            {
                AdditionalTypesRegistration = containerBuilder =>
                {
                    var luisServiceMock = this.GetLuisServiceMock();

                    containerBuilder
                        .Register(c => luisServiceMock.Object)
                        .Keyed<ILuisService>(FiberModule.Key_DoNotSerialize)
                        .As<ILuisService>()
                        .SingleInstance();

                    containerBuilder
                        .RegisterType<TDialog>()
                        .As<IDialog<object>>()
                        .InstancePerDependency();

                    this.RegisterAdditionalTypes(containerBuilder);
                }
            };


            return builder;
        }

        private Mock<ILuisService> GetLuisServiceMock()
        {
            var luisServiceMock = new Mock<ILuisService>(MockBehavior.Strict);

            foreach (var intentUtterance in this.intentUtterances)
            {
                var score = intentUtterance.Score ?? 1.0;

                SetupLuis(
                    luisServiceMock,
                    intentUtterance.Utterance,
                    intentUtterance.IntentAction,
                    score,
                    intentUtterance.Entities.ToArray());
            }

            return luisServiceMock;
        }
    }
}
