
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

public class ReadOnlyRecord
{
    [Member("id"), Key]
    public long Id { get; }

    [Member("type")]
    public RecordType Type { get; }
}
