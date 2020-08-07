using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BillingMode
    {
        PROVISIONED = 1,
        PAY_PER_REQUEST = 2
    };
}