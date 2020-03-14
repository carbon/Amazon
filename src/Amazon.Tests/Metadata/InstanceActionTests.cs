using System;
using System.Text.Json;

using Xunit;

namespace Amazon.Metadata.Tests
{
    public sealed class InstanceActionTests
    {
        // {"action": "stop", "time": "2017-09-18T08:22:00Z"}

        [Fact]
        public void Deserialize_Stop()
        {
            string text = @"{""action"": ""stop"", ""time"": ""2017-09-18T08:22:00Z""}";

            var action = JsonSerializer.Deserialize<InstanceAction>(text);

            Assert.Equal("stop", action.Action);
            Assert.Equal(new DateTime(2017, 09, 18, 08, 22, 00, DateTimeKind.Utc), action.Time);
        }

        [Fact]
        public void Deserialize_Terminate()
        {
            string text = @"{""action"": ""terminate"", ""time"": ""2017-09-18T08:22:00Z""}";

            var action = JsonSerializer.Deserialize<InstanceAction>(text);

            Assert.Equal("terminate", action.Action);
            Assert.Equal(new DateTime(2017, 09, 18, 08, 22, 00, DateTimeKind.Utc), action.Time);
        }

    }
}