using System;

namespace Amazon.Ses
{
    public class SesException : Exception
    {
        private readonly SesError error;

        public SesException(string message)
            : base(message) { }

        public SesException(SesError error)
            : this(error.Message)
        {
            this.error = error;
        }

        public string Type => error.Type;

        public string Code => error.Code;

        public bool IsTransient => Code == "Throttling" || Code == "ServiceUnavailable";
    }
}