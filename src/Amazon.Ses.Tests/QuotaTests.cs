namespace Amazon.Ses.Tests
{
    public class QuotaTests
    {
        [Fact]
        public void SesParseQuota()
        {
            var text = @"<GetSendQuotaResponse xmlns=""http://ses.amazonaws.com/doc/2010-12-01/"">
  <GetSendQuotaResult>
    <SentLast24Hours>127.0</SentLast24Hours>
    <Max24HourSend>200.0</Max24HourSend>
    <MaxSendRate>1.0</MaxSendRate>
  </GetSendQuotaResult>
  <ResponseMetadata>
    <RequestId>273021c6-c866-11e0-b926-699e21c3af9e</RequestId>
  </ResponseMetadata>
</GetSendQuotaResponse>";

            var getQuotaResponse = GetSendQuotaResponse.Parse(text);

            Assert.Equal(127f, getQuotaResponse.GetSendQuotaResult.SentLast24Hours);
            Assert.Equal(200f, getQuotaResponse.GetSendQuotaResult.Max24HourSend);
            Assert.Equal(1f, getQuotaResponse.GetSendQuotaResult.MaxSendRate);
        }
    }
}