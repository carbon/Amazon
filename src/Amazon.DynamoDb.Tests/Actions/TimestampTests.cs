namespace Amazon.DynamoDb.Models.Tests;

public class TimestampTests
{
    [Fact]
    public void CanConvertToDateTime()
    {
        var timestamp = new Timestamp(1658453070);

        Assert.Equal(1658453070d, timestamp.Value);

        Assert.Equal(new DateTime(2022, 07, 22, 01, 24, 30, DateTimeKind.Utc), timestamp);
    }

    [Fact]
    public void CanConvertToDateTimeOffset()
    {
        var timestamp = new Timestamp(1658453070);

        Assert.Equal(timestamp.Value, ((DateTimeOffset)timestamp).ToUnixTimeSeconds());
        Assert.Equal(new DateTimeOffset(2022, 07, 22, 01, 24, 30, TimeSpan.Zero), timestamp);    
    }
}