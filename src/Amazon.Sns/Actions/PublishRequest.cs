﻿namespace Amazon.Sns;

public sealed class PublishRequest
{
    public PublishRequest(
        string topicArn,
        string message,
        string? subject = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(topicArn);
        ArgumentException.ThrowIfNullOrEmpty(message);

        TopicArn = topicArn;
        Message = message;
        Subject = subject;
    }

    public string TopicArn { get; }

    public string Message { get; }

    public string? Subject { get; }

    internal List<KeyValuePair<string, string>> ToParams()
    {
        var dic = new List<KeyValuePair<string, string>>(5) {
            new ("Action", "Publish"),
            new ("TopicArn", TopicArn),
            new ("Message", Message)
        };

        if (Subject is not null)
        {
            dic.Add(new ("Subject", Subject));
        }

        return dic;
    }
}

/*
 http://sns.us-east-1.amazonaws.com/
  ?Subject=My%20first%20message
  &TopicArn=arn%3Aaws%3Asns%3Aus-east-1%3A698519295917%3AMy-Topic
  &Message=Hello%20world%21
  &Action=Publish
  &SignatureVersion=2
  &SignatureMethod=HmacSHA256
  &Timestamp=2010-03-31T12%3A00%3A00.000Z
  &AWSAccessKeyId=AKIAIOSFODNN7EXAMPLE
  &Signature=9GZysQ4Jpnz%2BHklqM7VFTvEcjR2LIUtn6jW47054xxE%3D
*/