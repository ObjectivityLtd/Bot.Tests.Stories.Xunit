namespace Objectivity.Bot.Tests.Stories.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [Serializable]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused constructors added to follow exception best practices")]
    public class UnmatchedUtteranceException : Exception
    {
        public UnmatchedUtteranceException()
        {
        }

        public UnmatchedUtteranceException(string message)
            : base(message)
        {
        }

        public UnmatchedUtteranceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected UnmatchedUtteranceException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
