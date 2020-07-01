using System.Linq;

using Xunit;

namespace Amazon.Tests
{
    public class AwsRegionTests
    {
        [Fact]
        public void Enum()
        {
            Assert.Equal(20, AwsRegion.All.Length);

            foreach (var region in AwsRegion.All)
            {
                Assert.Same(region, AwsRegion.Get(region.Name));
            }

            // Ensure there are not any dublicates
            var names = AwsRegion.All.Select(a => a.Name).Distinct().ToArray();

            Assert.Equal(AwsRegion.All.Length, names.Length);
        }

        [Fact]
        public void A()
        {
            var r1 = AwsRegion.Get("us-east-1");
            var r2 = AwsRegion.Get("us-east-2");

            Assert.Same(AwsRegion.USEast1, r1);
            Assert.Same(AwsRegion.USEast2, r2);

            Assert.True(r1 != r2);

            Assert.Equal("us-east-1", r1.ToString());
            Assert.Equal("us-east-2", r2.ToString());
        }

        [Fact]
        public void Equality()
        {
            Assert.True(AwsRegion.USEast1.Equals(AwsRegion.USEast1));
            Assert.False(AwsRegion.USEast1.Equals(AwsRegion.USEast2));
        }

        [Fact]
        public void B()
        {
            Assert.Same(AwsRegion.USEast1       , AwsRegion.Get("us-east-1"));
            Assert.Same(AwsRegion.USEast2       , AwsRegion.Get("us-east-2"));
            Assert.Same(AwsRegion.USWest1       , AwsRegion.Get("us-west-1"));
            Assert.Same(AwsRegion.USWest2       , AwsRegion.Get("us-west-2"));
            Assert.Same(AwsRegion.CACentral1    , AwsRegion.Get("ca-central-1"));
            Assert.Same(AwsRegion.APSouth1      , AwsRegion.Get("ap-south-1"));
            Assert.Same(AwsRegion.APSouthEast1  , AwsRegion.Get("ap-southeast-1"));
        }
    }
}
