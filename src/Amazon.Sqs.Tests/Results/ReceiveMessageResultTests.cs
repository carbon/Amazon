using System.Text.Json;

using Amazon.Sqs.Serialization;

namespace Amazon.Sqs.Results.Tests;

public class ReceiveMessageResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        ReceiveMessageResult? result = JsonSerializer.Deserialize(
            """
            {
              "Messages": [
                {
                  "MessageId": "770a44de-c6a7-41f8-88e9-02d4b48d185a",
                  "ReceiptHandle": "AQEBdB/NO+9Dl7fxnavwSb0cgpDZe7rFVhymlWIYEA9XLaZitTP3uquJ0pk31JtwO3FWuHQz+eA2wGbsVtqoPPjCIhegRbkhxp2abnMZt2fskWeHYmZjtW4tGZ40Y2Dfw/evml/QxGcVxff9Oug2IB4DG5vsbfAEBr8ccdFylX2PxRsPZvoSoR7NwLvJ1cvM55HXEUVtLiRTTj0ZK2fRVOQZVgp2Vo9s5MXDiJ8ybz2Mj3QZO2y0+4rHjyh04vdDlT1EiS1FwsIAzLTE3ixjlFWlPnwXaHoSwsl791CYCC3+dPj/Z//qMZ+Q/HX9HFUArJJkt3uoeQaIcGABXUInJzCHPB49IhmX63i3umiqIAJyqVRX/xV15GP9OwSa4sH8Cpqh",
                  "Body": "test 1",
                  "MD5OfBody": "9a7b64c98b066602b21f869ae7cd673a"
                }
              ]
            }
            """u8, SqsSerializerContext.Default.ReceiveMessageResult);

        Assert.NotNull(result);

        Assert.Single(result.Messages);

        Assert.StartsWith("AQEBdB", result.Messages[0].ReceiptHandle);
        Assert.Equal("test 1", result.Messages[0].Body);
    }

    [Fact]
    public void CanDeserializeAttributes()
    {
        ReceiveMessageResult? result = JsonSerializer.Deserialize(
            """
            {
              "Messages": [
                {
                  "Attributes": {
                    "SenderId": "AIDASSYFHUBOBT7F4XT75",
                    "ApproximateFirstReceiveTimestamp": "1677112433437",
                    "ApproximateReceiveCount": "1",
                    "SentTimestamp": "1677112427387"
                  },
                  "Body": "This is a test message",
                  "MD5OfBody": "fafb00f5732ab283681e124bf8747ed1",
                  "MessageId": "219f8380-5770-4cc2-8c3e-5c715e145f5e",
                  "ReceiptHandle": "AQEBaZ+j5qUoOAoxlmrCQPkBm9njMWXqemmIG6shMHCO6fV20JrQYg/AiZ8JELwLwOu5U61W+aIX5Qzu7GGofxJuvzymr4Ph53RiR0mudj4InLSgpSspYeTRDteBye5tV/txbZDdNZxsi+qqZA9xPnmMscKQqF6pGhnGIKrnkYGl45Nl6GPIZv62LrIRb6mSqOn1fn0yqrvmWuuY3w2UzQbaYunJWGxpzZze21EOBtywknU3Je/g7G9is+c6K9hGniddzhLkK1tHzZKjejOU4jokaiB4nmi0dF3JqLzDsQuPF0Gi8qffhEvw56nl8QCbluSJScFhJYvoagGnDbwOnd9z50L239qtFIgETdpKyirlWwl/NGjWJ45dqWpiW3d2Ws7q"
                }
              ]
            }
            """u8, SqsSerializerContext.Default.ReceiveMessageResult);

        Assert.NotNull(result);

        var message = result.Messages[0];

        Assert.Equal(4, message.Attributes.Count);
        Assert.Equal("AIDASSYFHUBOBT7F4XT75", message.Attributes["SenderId"]);
        Assert.Equal("This is a test message", message.Body);
        Assert.Equal("fafb00f5732ab283681e124bf8747ed1", message.MD5OfBody);
        Assert.Equal("219f8380-5770-4cc2-8c3e-5c715e145f5e", message.MessageId);
        Assert.StartsWith("AQEBaZ+j5q", message.ReceiptHandle);
    }
}