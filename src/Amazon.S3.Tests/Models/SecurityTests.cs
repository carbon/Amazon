namespace Amazon.S3.Tests
{
	using Xunit;

	public class SecurityTests
	{
		[Fact]
		public void SignRequest()
		{
			Assert.Equal("WzM6OJtOmiNYrFOSvypk3GjjyUM=", S3Helper.ComputeSignature("abc", "abc"));
		}
	}
}