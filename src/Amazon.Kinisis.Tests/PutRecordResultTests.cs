using System.Text.Json;

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordResultTests
    {
        [Fact]
        public void Deserialize()
        {
            string rawJson = @"{
               ""Encrypted"": false,
               ""RecordId"": ""id""
            }";

            var result = JsonSerializer.Deserialize<PutRecordResult>(rawJson);

            Assert.False(result.Encrypted);
            Assert.Equal("id", result.RecordId);
        }
    }
}