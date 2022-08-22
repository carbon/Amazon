﻿namespace Amazon.Sqs.Models.Tests;

public class SendMessageResultTests
{
    [Fact]
    public void Parse()
    {
        var response = SendMessageResponse.Deserialize(
            """
            <SendMessageResponse xmlns="http://queue.amazonaws.com/doc/2012-11-05/">
              <SendMessageResult>
                <MD5OfMessageBody>5d41402abc4b2a76b9719d911017c592</MD5OfMessageBody>
                <MessageId>cafaea9a-70f8-47c7-89b3-7bbb572cf061</MessageId>
              </SendMessageResult>
            </SendMessageResponse>
            """);
        
        var result = response.SendMessageResult;

        Assert.Equal("5d41402abc4b2a76b9719d911017c592",     result.MD5OfMessageBody);
        Assert.Equal("cafaea9a-70f8-47c7-89b3-7bbb572cf061", result.MessageId);
    }
}