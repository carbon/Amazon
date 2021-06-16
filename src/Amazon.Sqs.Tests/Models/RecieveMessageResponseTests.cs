using Xunit;

namespace Amazon.Sqs.Models.Tests
{
    public class RecieveMessageResponseTests
	{
		[Fact]
		public void Parse_1()
		{
			var xmlText =

            @"<ReceiveMessageResponse xmlns=""http://queue.amazonaws.com/doc/2012-11-05/"">
				<ReceiveMessageResult>
					<Message>
						<MessageId>12c1f093-643c-49e2-af28-94df4b9d6a84</MessageId>
						<ReceiptHandle>+eXJYhj5rDpwwfdMtBtd6QBhJntxdWbeBlW6Qrh9u7V0DJ3gVOmjI2Gh/oFnb0IeJqy5Zc8kH4KpI3G0WSKZVTaAPSeOkXQZ8qnqKILOutkJKuHVoL0gMga8sgFn9YRqcW9MErE7txwqrC6dW9o1KF3yLZ6j1BOptdq8UMyKMCs=</ReceiptHandle>
						<Body>hello</Body>		
					</Message>
				</ReceiveMessageResult>
				<ResponseMetadata>
					<RequestId>038693c3-beef-435e-a328-de8a890ced3d</RequestId>
				</ResponseMetadata>
			</ReceiveMessageResponse>";

			var result = ReceiveMessageResponse.ParseXml(xmlText);

            var messages = result.ReceiveMessageResult.Items;

			Assert.Equal("12c1f093-643c-49e2-af28-94df4b9d6a84", messages[0].MessageId);


			Assert.Equal(
				expected:	"+eXJYhj5rDpwwfdMtBtd6QBhJntxdWbeBlW6Qrh9u7V0DJ3gVOmjI2Gh/oFnb0IeJqy5Zc8kH4KpI3G0WSKZVTaAPSeOkXQZ8qnqKILOutkJKuHVoL0gMga8sgFn9YRqcW9MErE7txwqrC6dW9o1KF3yLZ6j1BOptdq8UMyKMCs=",
				actual:		messages[0].ReceiptHandle
			);

			Assert.Equal("hello", messages[0].Body);
		}

        [Fact]
        public void Parse_2()
        {
            var xmlText =

            @"<ReceiveMessageResponse xmlns=""http://queue.amazonaws.com/doc/2012-11-05/"">
				<ReceiveMessageResult>
					<Message>
						<MessageId>12c1f093-643c-49e2-af28-94df4b9d6a84</MessageId>
						<ReceiptHandle>+eXJYhj5rDpwwfdMtBtd6QBhJntxdWbeBlW6Qrh9u7V0DJ3gVOmjI2Gh/oFnb0IeJqy5Zc8kH4KpI3G0WSKZVTaAPSeOkXQZ8qnqKILOutkJKuHVoL0gMga8sgFn9YRqcW9MErE7txwqrC6dW9o1KF3yLZ6j1BOptdq8UMyKMCs=</ReceiptHandle>
						<MD5OfBody>5d41402abc4b2a76b9719d911017c592</MD5OfBody>
						<Body>a</Body>		
                        <SequenceNumber>1</SequenceNumber>
					</Message>
                    <Message>
						<MessageId>2</MessageId>
						<ReceiptHandle>+eXJYhj5rDpwwfdMtBtd6QBhJntxdWbeBlW6Qrh9u7V0DJ3gVOmjI2Gh/oFnb0IeJqy5Zc8kH4KpI3G0WSKZVTaAPSeOkXQZ8qnqKILOutkJKuHVoL0gMga8sgFn9YRqcW9MErE7txwqrC6dW9o1KF3yLZ6j1BOptdq8UMyKMCs=</ReceiptHandle>
						<MD5OfBody>5d41402abc4b2a76b9719d911017c592</MD5OfBody>
						<Body>b</Body>		
                        <SequenceNumber>2</SequenceNumber>
					</Message>
				</ReceiveMessageResult>
			</ReceiveMessageResponse>";

            var messages = ReceiveMessageResponse.ParseXml(xmlText).ReceiveMessageResult.Items;

            Assert.Equal(2, messages.Length);
            Assert.Equal("a", messages[0].Body);
            Assert.Equal("1", messages[0].SequenceNumber);
            Assert.Equal("b", messages[1].Body);
            Assert.Equal("2", messages[1].SequenceNumber);
        }

        [Fact]
		public void Parse_3()
		{
            string xmlText =

                @"<ReceiveMessageResponse xmlns=""http://queue.amazonaws.com/doc/2012-11-05/"">
  <ReceiveMessageResult>
    <Message>
      <MessageId>5fea7756-0ea4-451a-a703-a558b933e274</MessageId>
      <ReceiptHandle>
        MbZj6wDWli+JvwwJaBV+3dcjk2YW2vA3+STFFljTM8tJJg6HRG6PYSasuWXPJB+Cw
        Lj1FjgXUv1uSj1gUPAWV66FU/WeR4mq2OKpEGYWbnLmpRCJVAyeMjeU5ZBdtcQ+QE
        auMZc8ZRv37sIW2iJKq3M9MFx1YvV11A2x/KSbkJ0=
      </ReceiptHandle>
      <MD5OfBody>fafb00f5732ab283681e124bf8747ed1</MD5OfBody>
      <Body>This is a test message</Body>
      <Attribute>
        <Name>SenderId</Name>
        <Value>195004372649</Value>
      </Attribute>
      <Attribute>
        <Name>SentTimestamp</Name>
        <Value>1238099229000</Value>
      </Attribute>
      <Attribute>
        <Name>ApproximateReceiveCount</Name>
        <Value>5</Value>
      </Attribute>
      <Attribute>
        <Name>ApproximateFirstReceiveTimestamp</Name>
        <Value>1250700979248</Value>
      </Attribute>
    </Message>
  </ReceiveMessageResult>
  <ResponseMetadata>
    <RequestId>b6633655-283d-45b4-aee4-4e84e0ae6afa</RequestId>
  </ResponseMetadata>
</ReceiveMessageResponse>";

            var messages = ReceiveMessageResponse.ParseXml(xmlText).ReceiveMessageResult.Items;

            var message = messages[0];

            var a_0 = message.Attributes[0];
            var a_1 = message.Attributes[1];

            Assert.Equal("SenderId",     a_0.Name);
            Assert.Equal("195004372649", a_0.Value);

            Assert.Equal("SentTimestamp", a_1.Name);
            Assert.Equal("1238099229000", a_1.Value);

            Assert.Equal(4, message.Attributes.Length);
        }
    }
}