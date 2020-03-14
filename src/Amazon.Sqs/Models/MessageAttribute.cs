using System;
using System.Globalization;

namespace Amazon.Sqs
{
    public readonly struct MessageAttribute
    {
        public MessageAttribute(string name, string value)
        {
            Name = name;
            Value = new MessageAttributeValue(MessageAttributeDataType.String, value);
        }

        public MessageAttribute(string name, long value)
        {
            Name = name;
            Value = new MessageAttributeValue(MessageAttributeDataType.Number, value.ToString(CultureInfo.InvariantCulture));

        }

        public MessageAttribute(string name, byte[] value)
        {
            Name = name;
            Value = new MessageAttributeValue(MessageAttributeDataType.Binary, Convert.ToBase64String(value));
        }

        public string Name { get; }

        public MessageAttributeValue Value { get; }
    }

    public readonly struct MessageAttributeValue
    {
        public MessageAttributeValue(MessageAttributeDataType type, string value)
        {
            DataType = type;
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        // Encoded as StringValue | BinaryValue
        public string Value { get; }

        public MessageAttributeDataType DataType { get; }
    }

    public enum MessageAttributeDataType
    {
        String = 1,
        Number = 2, // A number can have up to 38 digits of precision, and it can be between 10^-128 and 10^+126.
        Binary = 3
    }
}