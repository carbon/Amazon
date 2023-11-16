using System.Text.Json.Serialization;

namespace Amazon.Sqs;

[JsonConverter(typeof(JsonStringEnumConverter<MessageAttributeDataType>))]
public enum MessageAttributeDataType
{
    String = 1,
    Number = 2, // A number can have up to 38 digits of precision, and it can be between 10^-128 and 10^+126.
    Binary = 3
}