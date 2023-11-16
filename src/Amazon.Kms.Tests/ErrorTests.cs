using System.Text.Json;

using Amazon.Kms.Exceptions;
using Amazon.Kms.Serialization;

namespace Amazon.Kms.Tests;

public class ErrorTests
{
    [Fact]
    public void ParseValidationError()
    {
        KmsError? error = JsonSerializer.Deserialize(
            """
            {
              "__type":"ValidationException",
              "message":"1 validation error detected: Value '0' at 'limit' failed to satisfy constraint: Member must have value greater than or equal to 1"
            }
            """, KmsSerializerContext.Default.KmsError);

        Assert.NotNull(error);
        Assert.Equal("ValidationException", error.Type);
        Assert.Equal("1 validation error detected: Value '0' at 'limit' failed to satisfy constraint: Member must have value greater than or equal to 1", error.Message);
    }
}
