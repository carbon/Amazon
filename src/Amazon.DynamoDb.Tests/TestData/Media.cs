
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("Media")]
public class Media
{
    [Member("Transformation"), Key]
    public string Transformation { get; set; }

    [Member("ErrorMessage")]
    public string ErrorMessage { get; set; }

    [Member("Hash")]
    public byte[] Hash { get; set; }

    [Member("Duration")]
    [TimePrecision(TimePrecision.Millisecond)]
    public TimeSpan Duration { get; set; }

    [Member("Created")]
    [TimePrecision(TimePrecision.Second)]
    public DateTime Created { get; set; }
}
