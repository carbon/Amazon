using System.Text.Json;

using Amazon.Kms.Serialization;

namespace Amazon.Kms.Actions.Tests;

public class ListGrantsRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new ListGrantsRequest("1234abcd-12ab-34cd-56ef-1234567890ab");

        Assert.Equal(
            """
            {"KeyId":"1234abcd-12ab-34cd-56ef-1234567890ab"}
            """u8, JsonSerializer.SerializeToUtf8Bytes(request, KmsSerializerContext.Default.ListGrantsRequest));
    }
}