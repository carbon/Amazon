﻿namespace Amazon.S3.Models.Tests;

public class InitiateMultipartUploadResultTests
{
    [Fact]
    public void Deserialize()
    {
        var result = InitiateMultipartUploadResult.ParseXml(
            """
            <InitiateMultipartUploadResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
              <Bucket>example-bucket</Bucket>
              <Key>example-object</Key>
              <UploadId>VXBsb2FkIElEIGZvciA2aWWpbmcncyBteS1tb3ZpZS5tMnRzIHVwbG9hZA</UploadId>
            </InitiateMultipartUploadResult>
            """);

        Assert.Equal("example-bucket", result.Bucket);
        Assert.Equal("example-object", result.Key);
        Assert.Equal("VXBsb2FkIElEIGZvciA2aWWpbmcncyBteS1tb3ZpZS5tMnRzIHVwbG9hZA", result.UploadId);
    }
}