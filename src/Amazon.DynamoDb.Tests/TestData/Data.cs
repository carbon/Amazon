using System.Runtime.Serialization;

using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("Things")]
public class Thing
{
    [Member("color"), Key]
    public Color Color { get; set; }

    [Member("color2")]
    public ABCEnum X { get; set; }

    [Member("color3")]
    public ABCEnum? Y { get; set; }
}

public class Fruit
{
    [Member("name"), Key]
    [IgnoreDataMember]
    public string Name { get; init; }

    [Member("calories")]
    public int Calories { get; init; }

    [Member("description")]
    public string Description { get; init; }

    public bool IsHealthy { get; init; }
}

public enum RecordType
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3
}

[Dataset("Hi")]
public class Hi
{
    [Member("a"), Key]
    public Nested A { get; set; }

    [Member("b")]
    public Nested B { get; set; }
}

[Dataset("Nested")]
public class Nested
{
    [Member("a"), Key]
    public long A { get; set; }

    [Member("b")]
    public string B { get; set; }
}

public struct Position
{
    [Member("x"), Key]
    public float X { get; set; }

    [Member("y"), Key]
    public float Y { get; set; }

    [Member("z"), Key]
    public float? Z { get; set; }
}

[Dataset("BlobMetadata")]
public class Meta
{
    [Member(1), Key]
    public byte[] Id { get; set; }

    [Member(2)]
    public string Name { get; set; }

    [Member(3)]
    public Annotation[] Annotations { get; set; }
}