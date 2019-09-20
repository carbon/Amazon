using System;

namespace Amazon.Ses
{
    public sealed class SesException : Exception
    {
        private readonly SesError error;

        public SesException(SesError error)
            : base(error.Message)
        {
            this.error = error;
        }

        public string Type => error.Type;

        public string Code => error.Code;

        public bool IsTransient => Code == "Throttling" || Code == "ServiceUnavailable";
    }
}