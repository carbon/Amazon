using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Exceptions.Tests;

public class KmsErrorTests
{
    [Fact]
    public void CanDeserialize()
    {
        var text = """{"__type":"AccessDeniedException","message":"The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access."}""";

        KmsError? json = JsonSerializer.Deserialize(text, KmsSerializerContext.Default.KmsError);

        Assert.NotNull(json);
        Assert.Equal("AccessDeniedException", json.Type);
        Assert.Equal("The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access.", json.Message);
    }        
}