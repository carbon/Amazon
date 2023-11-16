using System.Text.Json;

using Amazon.Metadata.Serialization;

namespace Amazon.Metadata.Tests;

public sealed class InstanceActionTests
{
    [Fact]
    public void CanDeserialize_Stop()
    {
        var text = """{"action": "stop", "time": "2017-09-18T08:22:00Z"}""";

        var action = JsonSerializer.Deserialize(text, MetadataSerializerContext.Default.InstanceAction);

        Assert.NotNull(action);
        Assert.Equal("stop", action.Action);
        Assert.Equal(new DateTime(2017, 09, 18, 08, 22, 00, DateTimeKind.Utc), action.Time);
    }

    [Fact]
    public void CanDeserialize_Terminate()
    {
        var action = JsonSerializer.Deserialize(
            """
            {
              "action": "terminate",
              "time": "2017-09-18T08:22:00Z"
            }
            """, MetadataSerializerContext.Default.InstanceAction);

        Assert.NotNull(action);
        Assert.Equal("terminate", action.Action);
        Assert.Equal(new DateTime(2017, 09, 18, 08, 22, 00, DateTimeKind.Utc), action.Time);
    }
}