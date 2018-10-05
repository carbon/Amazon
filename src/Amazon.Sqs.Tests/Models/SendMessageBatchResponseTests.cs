using Xunit;

namespace Amazon.Sqs.Models.Tests
{	
	public class SendMessageBatchResponseTests
	{
		[Fact]
		public void SendMessageBatchResponseParse()
		{
            var xmlText = $@"
<SendMessageBatchResponse xmlns=""{SqsClient.NS}"">
	<SendMessageBatchResult>
		<SendMessageBatchResultEntry>
			<Id>test_msg_001</Id>
			<MessageId>0a5231c7-8bff-4955-be2e-8dc7c50a25fa</MessageId>
			<MD5OfMessageBody>0e024d309850c78cba5eabbeff7cae71</MD5OfMessageBody>
		</SendMessageBatchResultEntry>
		<SendMessageBatchResultEntry>
			<Id>test_msg_002</Id>
			<MessageId>15ee1ed3-87e7-40c1-bdaa-2e49968ea7e9</MessageId>
			<MD5OfMessageBody>7fb8146a82f95e0af155278f406862c2</MD5OfMessageBody>
		</SendMessageBatchResultEntry>
	</SendMessageBatchResult>

	<ResponseMetadata>
		<RequestId>ca1ad5d0-8271-408b-8d0f-1351bf547e74</RequestId>
	</ResponseMetadata>
</SendMessageBatchResponse>".Trim();

            var response = SendMessageBatchResponse.Parse(xmlText);

            var messages = response.SendMessageBatchResult.Items;
            
			Assert.Equal(2, messages.Length);

            var message_0 = messages[0];

            Assert.Equal("test_msg_001", message_0.Id);
            Assert.Equal("0a5231c7-8bff-4955-be2e-8dc7c50a25fa", message_0.MessageId);
            Assert.Equal("0e024d309850c78cba5eabbeff7cae71", message_0.MD5OfMessageBody);

            var message_1 = messages[1];

            Assert.Equal("test_msg_002", message_1.Id);
            Assert.Equal("15ee1ed3-87e7-40c1-bdaa-2e49968ea7e9", message_1.MessageId);
            Assert.Equal("7fb8146a82f95e0af155278f406862c2", message_1.MD5OfMessageBody);
        }
	}
}