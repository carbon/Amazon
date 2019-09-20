#nullable disable

namespace Amazon.Sqs.Models
{
    public class MessageAttributeValue
    {
        public string BinaryValue { get; set; }

        public string StringValue { get; set; }

        public string DataType { get; set; } // String, Number, and Binary
    }
}