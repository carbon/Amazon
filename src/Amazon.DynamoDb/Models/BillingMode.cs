using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BillingMode
    {
        PROVISIONED,
        PAY_PER_REQUEST,
    };
}