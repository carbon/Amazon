using Xunit;

namespace Amazon.Route53.Tests
{
    public class ChangeResourceRecordSetsResponseTests
    {
        [Fact]
        public void Deserialize()
        {
            string text = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<ChangeResourceRecordSetsResponse xmlns=""{Route53Client.Namespace}"">
   <ChangeInfo>
      <Comment>string</Comment>
      <Id>string</Id>
      <Status>string</Status>
   </ChangeInfo>
</ChangeResourceRecordSetsResponse>";

            var result = Route53Serializer<ChangeResourceRecordSetsResponse>.DeserializeXml(text);

            Assert.Equal("string", result.ChangeInfo.Status);
            Assert.Equal("string", result.ChangeInfo.Id);
        }
    }
}