﻿namespace Objectivity.Bot.Tests.Stories.Player
{
    using System.Threading;
    using System.Threading.Tasks;
    using StoryModel;

    public interface IStoryPlayer<T>
    {
        Task Play(IStory<T> story, CancellationToken cancellationToken = default(CancellationToken));
    }
}