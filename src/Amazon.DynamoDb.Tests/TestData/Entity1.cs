
using Carbon.Data.Annotations;
using Carbon.Data.Sequences;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("Entity")]
public class Entity
{
    [Member("id"), Key]
    public Uid Id { get; set; }
}
