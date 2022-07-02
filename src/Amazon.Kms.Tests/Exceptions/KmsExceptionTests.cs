#pragma warning disable IDE0150 // Prefer 'null' check over type check

using System.Net;

using Amazon.Exceptions;

namespace Amazon.Kms.Exceptions.Tests;

public class KmsExceptionTests
{
    [Fact]
    public void CanConstruct()
    {
        var error = new KmsError {
            Type = "KeyUnavailable", 
            Message = "The key is not available" 
        };

        var ex = new KeyUnavailableException(error, HttpStatusCode.NotFound);

        Assert.Equal("KeyUnavailable", ex.Type);
        Assert.Equal("The key is not available", ex.Message);
        Assert.Equal(HttpStatusCode.NotFound, ex.HttpStatusCode);

        Assert.True(ex is AwsException);

        Assert.True(ex.IsTransient);
    }
}