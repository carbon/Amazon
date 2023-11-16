using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class SqsClientTests
{
    [Fact]
    public void CanConstructPostRequest()
    {
        var client = new SqsClient(AwsRegion.USEast1, new AwsCredential("public", "private"));

        var request = client.ConstructPostRequest("DeleteMessage", new DeleteMessageRequest("queue-url", "receipt-handle"), SqsSerializerContext.Default.DeleteMessageRequest);

        Assert.NotNull(request.RequestUri);
        Assert.Equal("https://sqs.us-east-1.amazonaws.com/", request.RequestUri.OriginalString);
        Assert.Same(HttpMethod.Post,                         request.Method);
        Assert.Equal("AmazonSQS.DeleteMessage",              request.Headers.GetValues("X-Amz-Target").First());
        Assert.Equal("application/x-amz-json-1.0",           request.Content!.Headers.GetValues("Content-Type").First());
    }
}
