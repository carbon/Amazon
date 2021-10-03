using System.Text.Json;

namespace Amazon.Kms.Tests
{
    public class CreateGrantTests
    {
        [Fact]
        public void Serialize()
        {
            var request = new CreateGrantRequest
            {
                KeyId = "key",
                GranteePrincipal = "principle",
                Operations = new[] { KmsOperations.Decrypt },
                Constraints = new GrantConstraints {
                    EncryptionContextEquals = new Dictionary<string, string> {
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
  ""Operations"": [
    ""Decrypt""
  ]
}", JsonSerializer.Serialize(request, JSO.Default));
        }
    }
}