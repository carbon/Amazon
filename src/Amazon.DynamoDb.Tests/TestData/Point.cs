
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

public class Point
{
    [Member("1"), Key]
    public float X { get; set; }

    [Member("2"), Key]
    public float Y { get; set; }

    [Member("3"), Key]
    public float Z { get; set; }
}
