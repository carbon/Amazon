using System;

namespace Amazon.Sts.Exceptions
{
    public sealed class StsException : Exception
    {
        public StsException(string message)
            : base(message) { }
    }
}
