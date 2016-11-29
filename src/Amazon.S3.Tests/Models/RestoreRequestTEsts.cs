using Xunit;

namespace Amazon.S3.Models.Tests
{
    public class RestoreRequestTests
    {
        [Fact]
        public void RestoreRequest()
        {
            var r = new RestoreObjectRequest(AwsRegion.USEast1, "a", "hi", 30);

            Assert.Equal(@"<RestoreRequest>
   <Days>30</Days>
</RestoreRequest>", r.GetXmlString());
        }
    }
}
