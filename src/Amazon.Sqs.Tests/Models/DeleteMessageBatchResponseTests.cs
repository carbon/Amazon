using Xunit;

namespace Amazon.Sqs.Models.Tests
{
    public class DeleteMessageBatchResponseTests
	{
		[Fact]
		public void Parse()
		{
            string xmlText = @"
<DeleteMessageBatchResponse xmlns=""http://queue.amazonaws.com/doc/2012-11-05/"">
    <DeleteMessageBatchResult>
        <DeleteMessageBatchResultEntry>
            <Id>msg1</Id>
        </DeleteMessageBatchResultEntry>
        <DeleteMessageBatchResultEntry>
            <Id>msg2</Id>
        </DeleteMessageBatchResultEntry>
    </DeleteMessageBatchResult>
    <ResponseMetadata>
        <RequestId>d6f86b7a-74d1-4439-b43f-196a1e29cd85</RequestId>
    </ResponseMetadata>
</DeleteMessageBatchResponse>".Trim();

            var result = DeleteMessageBatchResponse.Parse(xmlText).DeleteMessageBatchResult.Items;

            Assert.Equal(2, result.Length);

            Assert.Equal("msg1", result[0].Id);
            Assert.Equal("msg2", result[1].Id);
        }
    }
}
