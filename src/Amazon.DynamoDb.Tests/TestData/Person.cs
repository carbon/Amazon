
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("People")]
public class Person
{
    [Member("Name"), Key]
    public string Name { get; set; }

    [Member("Age")]
    public int Age { get; set; }
}
