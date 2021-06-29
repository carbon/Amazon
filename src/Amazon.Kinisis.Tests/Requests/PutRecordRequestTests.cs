using System.Text;
using System.Text.Json;

using Xunit;

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var putRecord = new PutRecordRequest("Events", new Record(Encoding.UTF32.GetBytes("hello")));

            Assert.Equal(@"{
  ""DeliveryStreamName"": ""Events"",
  ""Record"": {
    ""Data"": ""aAAAAGUAAABsAAAAbAAAAG8AAAA=""
  }
}", JsonSerializer.Serialize(putRecord, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}