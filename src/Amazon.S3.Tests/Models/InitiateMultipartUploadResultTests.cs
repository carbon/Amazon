namespace Amazon.S3.Models.Tests
{
	using Xunit;
	
	public class InitiateMultipartUploadResultTests
	{
		[Fact]
		public void CompleteXmlGenerate()
		{
			var g = new CompleteMultipartUpload(new[] { 
				new UploadPartResult(1, "uploadId", "eTag1"), 
				new UploadPartResult(2, "uploadId", "eTag2"), 
				new UploadPartResult(3, "uploadId", "eTag3")}
			);

			Assert.Equal(@"<CompleteMultipartUpload>
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
</CompleteMultipartUpload>", g.ToXmlString());
			
		}

		[Fact]
		public void ParseXml2()
		{
			string xmlText = 
			
			@"<InitiateMultipartUploadResult xmlns=""http://s3.amazonaws.com/doc/2006-03-01/"">
  <Bucket>example-bucket</Bucket>
  <Key>example-object</Key>
  <UploadId>VXBsb2FkIElEIGZvciA2aWWpbmcncyBteS1tb3ZpZS5tMnRzIHVwbG9hZA</UploadId>
</InitiateMultipartUploadResult>";

			var result = InitiateMultipartUploadResult.ParseXml(xmlText);

			Assert.Equal("example-bucket", result.Bucket);
			Assert.Equal("example-object", result.Key);
			Assert.Equal("VXBsb2FkIElEIGZvciA2aWWpbmcncyBteS1tb3ZpZS5tMnRzIHVwbG9hZA", result.UploadId);
		}
	}
}