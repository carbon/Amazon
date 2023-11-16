using System.Diagnostics.CodeAnalysis;
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
}