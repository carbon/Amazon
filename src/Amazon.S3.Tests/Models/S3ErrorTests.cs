namespace Amazon.S3.Tests
{
	using Xunit;

	public class S3ErrorTests
	{
		[Fact]
		public void ParseS3ErrorXml()
		{
			string xmlText =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<Error>
  <Code>NoSuchKey</Code>
  <Message>The resource you requested does not exist</Message>
  <Resource>/mybucket/myfoto.jpg</Resource> 
  <RequestId>4442587FB7D0A2F9</RequestId>
</Error>";
			var error = S3Error.ParseXml(xmlText);

			Assert.Equal("NoSuchKey", error.Code);
			Assert.Equal("The resource you requested does not exist", error.Message);
			Assert.Equal("/mybucket/myfoto.jpg", error.Resource);
			Assert.Equal("4442587FB7D0A2F9", error.RequestId);
		}

		[Fact]
		public void ParseS3Error2()
		{
			string xmlText = 
@"<Error>
	<Code>BadDigest</Code>
	<Message>The Content-MD5 you specified did not match what we received.</Message>
	<RequestId>FE689C8C5E73D8B6</RequestId>
	<CalculatedDigest>1B2M2Y8AsgTpgAmY7PhCfg==</CalculatedDigest>
	<ExpectedDigest>d41d8cd98f00b204e9800998ecf8427e</ExpectedDigest>
	<HostId>3oDR1WldBiVFbMm4EKHrDb4W1haIG5Z8uXYwLuSc/Z1/YWupOApcCtnwmneoAYN4</HostId>
</Error>";
			var error = S3Error.ParseXml(xmlText);

			Assert.Equal("BadDigest", error.Code);
			Assert.Equal("The Content-MD5 you specified did not match what we received.", error.Message);
			Assert.Equal(null, error.Resource);
			Assert.Equal("FE689C8C5E73D8B6", error.RequestId);
		}

	}
}