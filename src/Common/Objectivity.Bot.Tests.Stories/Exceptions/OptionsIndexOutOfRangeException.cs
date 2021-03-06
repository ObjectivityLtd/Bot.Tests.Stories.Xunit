﻿namespace Objectivity.Bot.Tests.Stories.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    [Serializable]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Unused constructors added to follow exception best practices")]
    public class OptionsIndexOutOfRangeException : Exception
    {
        private const string ExceptionMessage = "Error while performing a user step of test story: couldn't retrieve index {0} from options array containing {1} elements.";

        public OptionsIndexOutOfRangeException(int index, int optionsCount)
            : base(string.Format(CultureInfo.InvariantCulture, ExceptionMessage, index, optionsCount))
        {
        }

        public OptionsIndexOutOfRangeException(string message)
            : base(message)
        {
        }

        public OptionsIndexOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
