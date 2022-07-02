namespace Amazon.DynamoDb.Models.Tests;

public class AttributeWriterTests
{
    [Fact]
    public void CanWriteValues()
    {
        var sb = new StringWriter();

        var writer = new AttributeWriter(sb);

        var value = new DbValue(new AttributeCollection {
            { "a", 1 },
            { "b", "boat" }
            });


        writer.WriteDbValue(value);

        Assert.Equal("""{"M":{"a":{"N":"1"},"b":{"S":"boat"}}}""", sb.ToString());
    }
}