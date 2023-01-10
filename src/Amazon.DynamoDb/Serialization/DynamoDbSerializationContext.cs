using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Serialization;

[JsonSerializable(typeof(AttributeCollection))]
[JsonSerializable(typeof(DbValue))]
[JsonSerializable(typeof(Timestamp))]
[JsonSerializable(typeof(PutRequest))]
[JsonSerializable(typeof(DeleteRequest))]
[JsonSerializable(typeof(BatchGetItemRequest))]
public partial class DynamoDbSerializationContext : JsonSerializerContext
{
}