namespace Amazon.Kms.Tests
{
    public class KmsClientTests
    {
        [Fact]
        public void Version()
        {
            Assert.Equal("2014-11-01", KmsClient.Version);
        }
    }
}