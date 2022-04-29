namespace Amazon.Ses.Tests;

public class SendEmailResponseTests
{
    [Fact]
    public void CanParse()
    {
        var text = @"<SendEmailResponse xmlns=""http://ses.amazonaws.com/doc/2010-12-01/"">
  <SendEmailResult>
    <MessageId>000001438f3bba7a-a9f60f43-8bb0-417e-91a0-3aedeb0bd995-000000</MessageId>
  </SendEmailResult>
  <ResponseMetadata>
    <RequestId>209d1989-7cdd-11e3-95cd-dd6ca710a9a6</RequestId>
  </ResponseMetadata>
</SendEmailResponse>";

        var response = SendEmailResponse.Parse(text);

        Assert.Equal("000001438f3bba7a-a9f60f43-8bb0-417e-91a0-3aedeb0bd995-000000", response.SendEmailResult.MessageId);
    }
}