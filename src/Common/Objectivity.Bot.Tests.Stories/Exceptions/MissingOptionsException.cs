namespace Objectivity.Bot.Tests.Stories.Exceptions
{
    using System;

    public class MissingOptionsException : Exception
    {
        private const string ExceptionMessage = "Error while performing a user step of test story. Couldn't retrieve choice options from previous bot responses.";

        public MissingOptionsException()
            : base(ExceptionMessage)
        {
        }
    }
}
