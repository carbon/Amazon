using System.Text.Json;

using Xunit;

namespace Amazon.Kms.Tests
{
    public class GenerateDataKeyRequestTests
    {
        [Fact]
        public void A()
        {
            var request = new GenerateDataKeyRequest {
                KeyId = "1",
                KeySpec = KeySpec.AES_128,
                NumberOfBytes = 128 // 128, 256, 512, and 1024 
            };

            Assert.Equal(
@"{
  ""KeyId"": ""1"",
  ""KeySpec"": ""AES_128"",
  ""NumberOfBytes"": 128
}", JsonSerializer.Serialize(request, JSO.Default).ToString());
        }

        [Fact]
        public void B()
        {
            var request = new GenerateDataKeyRequest {
                KeyId = "1",
                KeySpec = KeySpec.AES_128
            };

            Assert.Equal(
@"{
  ""KeyId"": ""1"",
  ""KeySpec"": ""AES_128""
}", JsonSerializer.Serialize(request, JSO.Default));
        }
    }
}