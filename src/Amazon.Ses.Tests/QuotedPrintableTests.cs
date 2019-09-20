
using Xunit;

namespace Amazon.Ses.Tests
{
    public class QuotedPrintableTests
    {
        [Fact]
        public void Decode()
        {
            Assert.Equal("Subject", QuotedPrintable.Decode("=?utf-8?Q?Subject?="));
            Assert.Equal("☻",       QuotedPrintable.Decode("=?UTF-8?Q?=E2=98=BB?="));
        }

        [Fact]
        public void Encode()
        {   
            Assert.Equal("=?UTF-8?Q?=E2=98=BB?=", QuotedPrintable.Encode("☻"));
        }
    }
}