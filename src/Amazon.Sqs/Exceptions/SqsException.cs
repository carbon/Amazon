
using Amazon.Scheduling;

using Carbon.Messaging;

namespace Amazon.Sqs.Exceptions
{
    public sealed class SqsException : QueueException, IException
    {
        public SqsException(SqsError error)
            : base(error.Message)
        {
            Error = error;
        }

        public SqsError Error { get; }

        public bool IsTransient => false;
    }
}