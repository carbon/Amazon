using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReturnValuesOnConditionCheckFailure
    {
        /// <summary>
        /// If this parameter is not provided or is NONE, nothing is returned. 
        /// </summary>
        NONE = 0,

        /// <summary>
        /// ALL_OLD is specified, and UpdateItem overwrote an attribute name-value pair, the content of the old item is returned. 
        /// </summary>
        ALL_OLD = 1,
    }
}