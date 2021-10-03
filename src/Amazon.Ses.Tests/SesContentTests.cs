namespace Amazon.Ses.Tests
{
    public class SesContentTests
    {
        [Fact]
        public void SesContentEncoding()
        {
            Assert.Null(new SesContent("Hi").Charset);
            Assert.Equal("UTF-8", new SesContent("Hi", CharsetType.UTF8).Charset);
        }
    }
}