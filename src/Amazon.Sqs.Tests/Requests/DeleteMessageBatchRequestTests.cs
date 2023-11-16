using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class DeleteMessageBatchRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new DeleteMessageBatchRequest("https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/", [
            new DeleteMessageBatchRequestEntry("1", "AQEBaZ+j5qUoOAoxlmrCQPkBm9njMWXqemmIG6shMHCO6fV20JrQYg/AiZ8JELwLwOu5U61W+aIX5Qzu7GGofxJuvzymr4Ph53RiR0mudj4InLSgpSspYeTRDteBye5tV/txbZDdNZxsi+qqZA9xPnmMscKQqF6pGhnGIKrnkYGl45Nl6GPIZv62LrIRb6mSqOn1fn0yqrvmWuuY3w2UzQbaYunJWGxpzZze21EOBtywknU3Je/g7G9is+c6K9hGniddzhLkK1tHzZKjejOU4jokaiB4nmi0dF3JqLzDsQuPF0Gi8qffhEvw56nl8QCbluSJScFhJYvoagGnDbwOnd9z50L239qtFIgETdpKyirlWwl/NGjWJ45dqWpiW3d2Ws7q"),
            new DeleteMessageBatchRequestEntry("2", "r2")
        ]);

        Assert.Equal(
            """
            {
              "Entries": [
                {
                  "Id": "1",
                  "ReceiptHandle": "AQEBaZ+j5qUoOAoxlmrCQPkBm9njMWXqemmIG6shMHCO6fV20JrQYg/AiZ8JELwLwOu5U61W+aIX5Qzu7GGofxJuvzymr4Ph53RiR0mudj4InLSgpSspYeTRDteBye5tV/txbZDdNZxsi+qqZA9xPnmMscKQqF6pGhnGIKrnkYGl45Nl6GPIZv62LrIRb6mSqOn1fn0yqrvmWuuY3w2UzQbaYunJWGxpzZze21EOBtywknU3Je/g7G9is+c6K9hGniddzhLkK1tHzZKjejOU4jokaiB4nmi0dF3JqLzDsQuPF0Gi8qffhEvw56nl8QCbluSJScFhJYvoagGnDbwOnd9z50L239qtFIgETdpKyirlWwl/NGjWJ45dqWpiW3d2Ws7q"
                },
                {
                  "Id": "2",
                  "ReceiptHandle": "r2"
                }
              ],
              "QueueUrl": "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/"
            }
            """, request.ToIndentedJsonString(SqsSerializerContext.Default.DeleteMessageBatchRequest));
    }
}
