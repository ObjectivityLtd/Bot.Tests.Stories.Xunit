﻿namespace Objectivity.Bot.Tests.Stories.Recorder
{
    using Config;
    using StoryModel;

    public abstract class StoryRecorderBase<T> : IStoryRecorder<T>
    {
        protected StoryRecorderBase()
        {
            this.Bot = new BotRecorder<T>(this);
            this.User = new UserRecorder<T>(this);
            this.Config = new Config<T>(this);
        }

        public IBotRecorder<T> Bot { get; }

        public IUserRecorder<T> User { get; }

        public IConfig<T> Config { get; }

        public IStory<T> Story { get; } = new Story<T>();

        public IStory<T> Rewind()
        {
            return this.Story;
        }
    }
}