namespace Amazon.S3.Actions.Tests;

public class CompleteMultipartUploadTests
{
    [Fact]
    public void Serialize()
    {
        var g = new CompleteMultipartUpload(new UploadPartResult[] {
            new ("uploadId", 1, "eTag1"),
            new ("uploadId", 2, "eTag2"),
            new ("uploadId", 3, "eTag3")}
        );

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
            """, g.ToXmlString());
    }
}