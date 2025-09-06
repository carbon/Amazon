using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

[JsonConverter(typeof(JsonStringEnumConverter<IndexRotationPeriod>))]
public enum IndexRotationPeriod
{
    NoRotation = 1,
    OneHour = 2,
    OneDay = 3,
    OneWeek = 4,
    OneMonth = 5
}