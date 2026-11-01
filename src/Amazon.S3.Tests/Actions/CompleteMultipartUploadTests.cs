namespace Amazon.S3.Actions.Tests;

public class CompleteMultipartUploadTests
{
    [Fact]
    public async Task CanSerialize()
    {
        var request = new CompleteMultipartUploadRequest("s3.amazon.com", "bucket", "key", "uploadId", [
            new UploadPartResult("uploadId", 1, "eTag1"),
            new UploadPartResult("uploadId", 2, "eTag2"),
            new UploadPartResult("uploadId", 3, "eTag3")
        ]);

        byte[] data = await request.Content.ReadAsByteArrayAsync();

        Assert.Equal(
            """
            <CompleteMultipartUpload>
              <Part>
                <PartNumber>1</PartNumber>
                <ETag>eTag1</ETag>
              </Part>
              <Part>
                <PartNumber>2</PartNumber>
                <ETag>eTag2</ETag>
              </Part>
              <Part>
                <PartNumber>3</PartNumber>
                <ETag>eTag3</ETag>
              </Part>
            </CompleteMultipartUpload>
            """u8, data);
    }

    [Fact]
    public async Task RequestContentIsUtf8()
    {
        var request = new CompleteMultipartUploadRequest("s3.amazon.com", "bucket", "key", "uploadId", [
            new UploadPartResult("uploadId", 9, "é"),
            new UploadPartResult("uploadId", 10_000, "eTag2")
        ]);

        byte[] data = await request.Content.ReadAsByteArrayAsync();

        Assert.Equal(
            """
            <CompleteMultipartUpload>
              <Part>
                <PartNumber>9</PartNumber>
                <ETag>é</ETag>
              </Part>
              <Part>
                <PartNumber>10000</PartNumber>
                <ETag>eTag2</ETag>
              </Part>
            </CompleteMultipartUpload>
            """u8, data);

        Assert.Equal("text/xml; charset=utf-8", request.Content.Headers.ContentType.ToString());
    }
}
