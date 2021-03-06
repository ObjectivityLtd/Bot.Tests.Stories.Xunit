﻿namespace Objectivity.Bot.Tests.Stories.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused constructors added to follow exception best practices")]
    public class MissingOptionsException : Exception
    {
        private const string ExceptionMessage = "Error while performing a user step of test story. Couldn't retrieve choice options from previous bot responses.";

        public MissingOptionsException()
            : base(ExceptionMessage)
        {
        }

        public MissingOptionsException(string message)
            : base(message)
        {
        }

        public MissingOptionsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}