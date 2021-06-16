#nullable disable

namespace Amazon.Sqs.Models
{
    public sealed class BatchResultErrorEntry
    {
        public string Code { get; init; }

        public string Id { get; init; }

        public string Message { get; init; }

        public bool SenderFault { get; init; }
    }
}