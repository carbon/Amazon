#nullable disable

namespace Amazon.Ses
{
    public sealed class SesMail
    {
        public string Source { get; init; }

        public string[] Destination { get; init; }

        public string MessageId { get; init; }
    }
}