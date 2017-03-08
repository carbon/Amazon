using Xunit;

namespace Amazon.S3.Models.Tests
{
    public class RestoreRequestTests
    {
        [Fact]
        public void RestoreRequest()
        {
            var r = new RestoreObjectRequest(AwsRegion.USEast1, "a", "hi") { Days = 30 };

            Assert.Equal(@"<RestoreRequest>
  <Days>30</Days>
  <GlacierJobParameters>
    <Tier>Standard</Tier>
  </GlacierJobParameters>
</RestoreRequest>", r.GetXmlString());
        }
    }
}