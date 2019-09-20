#nullable disable

namespace Amazon.Ses
{
    public class SesMail
    {
        public string Source { get; set; }

        public string[] Destination { get; set; }

        public string MessageId { get; set; }
    }
}