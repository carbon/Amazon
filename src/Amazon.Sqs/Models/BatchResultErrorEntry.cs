#nullable disable

namespace Amazon.Sqs.Models
{
    public sealed class BatchResultErrorEntry
    {
        public string Code { get; set; }

        public string Id { get; set; }

        public string Message { get; set; }

        public bool SenderFault { get; set; }
    }
}