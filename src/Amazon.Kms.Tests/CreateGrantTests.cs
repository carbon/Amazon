using System.Text;

using Xunit;

using Carbon.Json;

namespace Amazon.Kms.Tests
{
    public class CreateGrantTests
    {
        [Fact]
        public void CreateGrantRequest()
        {
            var request = new CreateGrantRequest
            {
                KeyId = "key",
                GranteePrincipal = "principle",
                Operations = new[] { KmsOperations.Decrypt },
                Constraints = new GrantConstraints
                {
                    EncryptionContextEquals = new JsonObject {
                        { "vault", "master" }
                    }
                }
            };

            Assert.Equal(@"{
  ""Constraints"": {
    ""EncryptionContextEquals"": {
      ""vault"": ""master""
    }
  },
  ""GranteePrincipal"": ""principle"",
  ""KeyId"": ""key"",
  ""Operations"": [ ""Decrypt"" ]
}", JsonObject.FromObject(request).ToString());
        }


        [Fact]
        public void Error()
        {
            var text = "{\"__type\":\"AccessDeniedException\",\"Message\":\"The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access.\"}";

            var json = JsonObject.Parse(text).As<KmsError>();

            Assert.Equal("AccessDeniedException", json.Type);
            Assert.Equal("The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access.", json.Message);
        }
        [Fact]
        public void X()
        {
            // 128, 256, 512, and 1024 

            var request = new GenerateDataKeyRequest {
                KeyId         = "1",
                KeySpec       = KeySpec.AES_128,
                NumberOfBytes = 128
            };

            Assert.Equal(
@"{
  ""KeyId"": ""1"",
  ""KeySpec"": ""AES_128"",
  ""NumberOfBytes"": 128
}", JsonObject.FromObject(request).ToString());

        }

        [Fact]
        public void EncryptRequest()
        {
            // 128, 256, 512, and 1024. 1024 

            var request = new EncryptRequest {
                KeyId = "1",
                Plaintext = Encoding.UTF8.GetBytes("applesauce"),
                EncryptionContext = new JsonObject {
                    { "user", "1" }
                }
            };

            Assert.Equal(
@"{
  ""EncryptionContext"": {
    ""user"": ""1""
  },
  ""KeyId"": ""1"",
  ""Plaintext"": ""YXBwbGVzYXVjZQ==""
}", JsonObject.FromObject(request).ToString());

        }

        [Fact]
        public void X2()
        {
            var request = new GenerateDataKeyRequest {
                KeyId = "1",
                KeySpec = KeySpec.AES_128
            };

            Assert.Equal(
@"{
  ""KeyId"": ""1"",
  ""KeySpec"": ""AES_128""
}", JsonObject.FromObject(request).ToString());

        }        
    }
}