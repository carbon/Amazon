using Xunit;

namespace Amazon.S3.Models.Tests
{
    public class BatchDeleteTests
    {
        [Fact]
        public void Test()
        {
            var batch = new DeleteBatch("1", "2");

            Assert.Equal(expected: @"<Delete>
  <Object>
    <Key>1</Key>
  </Object>
  <Object>
    <Key>2</Key>
  </Object>
</Delete>",
          actual: batch.ToXmlString());
        }
    }
}