﻿using System.Text.Json;

namespace Amazon.Kms.Exceptions.Tests;

public class KmsErrorTests
{
    [Fact]
    public void CanDeserialize()
    {
        var text = """{"__type":"AccessDeniedException","message":"The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access."}""";

        var json = JsonSerializer.Deserialize<KmsError>(text);

        Assert.Equal("AccessDeniedException", json.Type);
        Assert.Equal("The ciphertext refers to a customer master key that does not exist, does not exist in this region, or you are not allowed to access.", json.Message);
    }        
}