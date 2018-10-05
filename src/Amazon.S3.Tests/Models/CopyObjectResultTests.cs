using System;

using Xunit;

namespace Amazon.S3.Models.Tests
{
    public class CopyObjectResultTests
    {
        [Fact]
        public void ParseXml()
        {
            string xmlText =

            @"<CopyObjectResult xmlns=""http://s3.amazonaws.com/doc/2006-03-01/"">
				<LastModified>2008-02-20T22:13:01</LastModified>
				<ETag>""7e9c608af58950deeb370c98608ed097""</ETag>
			</CopyObjectResult>";

            var result = CopyObjectResult.ParseXml(xmlText);

            Assert.Equal(new DateTime(2008, 02, 20, 22, 13, 01, DateTimeKind.Utc), result.LastModified);
            Assert.Equal(@"""7e9c608af58950deeb370c98608ed097""", result.ETag);
        }
    }
}
