
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

public class Record
{
    [Member(1), Key]
    public long Id { get; set; }

    [Member("Type")]
    public RecordType Type { get; set; }
}
