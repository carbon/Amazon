using System.Globalization;

namespace Amazon.Sqs;

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
        ArgumentNullException.ThrowIfNull(value);

        DataType = type;
        Value = value;
    }

    // Encoded as StringValue | BinaryValue
    public string Value { get; }

    public MessageAttributeDataType DataType { get; }
}