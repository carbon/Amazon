using System.Text.Json;
using Amazon.Kms.Serialization;

namespace Amazon.Kms.Actions.Tests;

public class CreateAliasTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new CreateAliasRequest("target-key", "alias-name");

        Assert.Equal(
            """
            {"TargetKeyId":"target-key","AliasName":"alias-name"}
            """u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.CreateAliasRequest));
    }
}