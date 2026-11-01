using System.Globalization;

namespace Amazon.Sns;

public sealed class MessageAttributeValue
{
    private MessageAttributeValue(string dataType, string? stringValue, byte[]? binaryValue)
    {
        DataType = dataType;
        StringValue = stringValue;
        BinaryValue = binaryValue;
    }

    public string DataType { get; }

    public string? StringValue { get; }

    public byte[]? BinaryValue { get; }

    public static MessageAttributeValue FromString(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        return new MessageAttributeValue("String", value, null);
    }

    // A JSON array, formatted as a string. e.g. ["a", 1, true, null]
    public static MessageAttributeValue FromStringArray(string jsonArray)
    {
        ArgumentException.ThrowIfNullOrEmpty(jsonArray);

        return new MessageAttributeValue("String.Array", jsonArray, null);
    }

    public static MessageAttributeValue FromNumber(decimal value)
    {
        return new MessageAttributeValue("Number", value.ToString(CultureInfo.InvariantCulture), null);
    }

    public static MessageAttributeValue FromBinary(byte[] value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length is 0)
        {
            throw new ArgumentException("Must not be empty", nameof(value));
        }

        return new MessageAttributeValue("Binary", null, value);
    }
}

// https://docs.aws.amazon.com/sns/latest/api/API_MessageAttributeValue.html
