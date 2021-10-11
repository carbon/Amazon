
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("Smorsborgs")]
public class Smorsborg
{
    [Member("Int32"), Key]
    public int Int32 { get; set; }

    [Member("Int64")]
    public long Int64 { get; set; }

    [Member("Boolean")]
    public Boolean Boolean { get; set; }

    [Member("Date")]
    public DateTime Date { get; set; }
}
