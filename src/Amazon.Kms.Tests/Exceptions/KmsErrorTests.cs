using System.Text.Json;

using Xunit;

namespace Amazon.Kms.Exceptions.Tests
{
    public class KmsErrorTests
    {
        [Fact]
        public void Deserialize()
        {
            var text = "{\"__type\":\"AccessDeniedException\",\"Message\":\"The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access.\"}";

            var json = JsonSerializer.Deserialize<KmsError>(text);

            Assert.Equal("AccessDeniedException", json.Type);
            Assert.Equal("The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access.", json.Message);
        }        
    }
}