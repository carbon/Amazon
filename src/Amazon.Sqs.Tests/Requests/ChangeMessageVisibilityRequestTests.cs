using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Tests;

public class ChangeMessageVisibilityRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new ChangeMessageVisibilityRequest(
            "https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/", 
            "AQEBaZ+j5qUoOAoxlmrCQPkBm9njMWXqemmIG6shMHCO6fV20JrQYg/AiZ8JELwLwOu5U61W+aIX5Qzu7GGofxJuvzymr4Ph53RiR0mudj4InLSgpSspYeTRDteBye5tV/txbZDdNZxsi+qqZA9xPnmMscKQqF6pGhnGIKrnkYGl45Nl6GPIZv62LrIRb6mSqOn1fn0yqrvmWuuY3w2UzQbaYunJWGxpzZze21EOBtywknU3Je/g7G9is+c6K9hGniddzhLkK1tHzZKjejOU4jokaiB4nmi0dF3JqLzDsQuPF0Gi8qffhEvw56nl8QCbluSJScFhJYvoagGnDbwOnd9z50L239qtFIgETdpKyirlWwl/NGjWJ45dqWpiW3d2Ws7q", 
            TimeSpan.FromHours(12)
        );

        Assert.Equal(
            """
            {"QueueUrl":"https://sqs.us-east-1.amazonaws.com/177715257436/MyQueue/","ReceiptHandle":"AQEBaZ\u002Bj5qUoOAoxlmrCQPkBm9njMWXqemmIG6shMHCO6fV20JrQYg/AiZ8JELwLwOu5U61W\u002BaIX5Qzu7GGofxJuvzymr4Ph53RiR0mudj4InLSgpSspYeTRDteBye5tV/txbZDdNZxsi\u002BqqZA9xPnmMscKQqF6pGhnGIKrnkYGl45Nl6GPIZv62LrIRb6mSqOn1fn0yqrvmWuuY3w2UzQbaYunJWGxpzZze21EOBtywknU3Je/g7G9is\u002Bc6K9hGniddzhLkK1tHzZKjejOU4jokaiB4nmi0dF3JqLzDsQuPF0Gi8qffhEvw56nl8QCbluSJScFhJYvoagGnDbwOnd9z50L239qtFIgETdpKyirlWwl/NGjWJ45dqWpiW3d2Ws7q","VisibilityTimeout":43200}
            """, JsonSerializer.Serialize(request, SqsSerializerContext.Default.ChangeMessageVisibilityRequest));
    }

    [Fact]
    public void ConstructorEnsuresVisibilityTimeoutDoesNotExceedRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ChangeMessageVisibilityRequest("queue-url", "handle", TimeSpan.FromHours(13)));
        Assert.Throws<ArgumentOutOfRangeException>(() => new ChangeMessageVisibilityRequest("queue-url", "handle", TimeSpan.FromHours(-1)));
    }
}