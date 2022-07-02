﻿using System.Text.Json;

namespace Amazon.Metadata.Tests;

public sealed class InstanceActionTests
{
    [Fact]
    public void CanDeserialize_Stop()
    {
        var text = """{"action": "stop", "time": "2017-09-18T08:22:00Z"}""";

        var action = JsonSerializer.Deserialize<InstanceAction>(text);

        Assert.Equal("stop", action.Action);
        Assert.Equal(new DateTime(2017, 09, 18, 08, 22, 00, DateTimeKind.Utc), action.Time);
    }

    [Fact]
    public void CanDeserialize_Terminate()
    {
        var text = """{"action": "terminate", "time": "2017-09-18T08:22:00Z"}""";

        var action = JsonSerializer.Deserialize<InstanceAction>(text);

        Assert.Equal("terminate", action.Action);
        Assert.Equal(new DateTime(2017, 09, 18, 08, 22, 00, DateTimeKind.Utc), action.Time);
    }
}