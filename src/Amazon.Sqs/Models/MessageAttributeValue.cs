using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public readonly struct MessageAttributeValue
{
    public MessageAttributeValue() { }

    [SetsRequiredMembers]
    public MessageAttributeValue(string value)
    {
        DataType = MessageAttributeDataType.String;
        StringValue = value;
    }

    [SetsRequiredMembers]
    public MessageAttributeValue(long value)
    {
        DataType = MessageAttributeDataType.Number;
        StringValue = value.ToString(CultureInfo.InvariantCulture);
    }

    [SetsRequiredMembers]
    public MessageAttributeValue(int value)
    {
        DataType = MessageAttributeDataType.Number;
        StringValue = value.ToString(CultureInfo.InvariantCulture);
    }

    [SetsRequiredMembers]
    public MessageAttributeValue(byte[] value)
    {
        DataType = MessageAttributeDataType.Binary;
        BinaryValue = value;
    }

    [JsonPropertyName("DataType")]
    public required MessageAttributeDataType DataType { get; init; }

    [JsonPropertyName("StringValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StringValue { get; init; }

    [JsonPropertyName("BinaryValue")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? BinaryValue { get; init; }

    // BinaryListValues
    // StringListValues

    public static implicit operator MessageAttributeValue(long value)
    {
        return new MessageAttributeValue(value);
    }

    public static implicit operator MessageAttributeValue(int value)
    {
        return new MessageAttributeValue(value);
    }

    public static implicit operator MessageAttributeValue(string value)
    {
        return new MessageAttributeValue(value);
    }

    public static implicit operator MessageAttributeValue(byte[] value)
    {
        return new MessageAttributeValue(value);
    }
}