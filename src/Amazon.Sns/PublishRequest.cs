using System;
using System.Collections.Generic;

namespace Amazon.Sns
{
    public sealed class PublishRequest
    {
        public PublishRequest(
            string topicArn, 
            string message,
            string? subject = null)
        {
            TopicArn = topicArn ?? throw new ArgumentNullException(nameof(topicArn));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Subject = subject;
        }

        public string TopicArn { get; }

        public string Message { get; }

        public string? Subject { get; }

        public Dictionary<string, string> ToParams()
        {
            var dic = new Dictionary<string, string>(4) {
                { "Action", "Publish" },
                { "TopicArn", TopicArn },
                { "Message", Message }
            };

            if (Subject != null)
            {
                dic.Add("Subject", Subject);
            }

            return dic;
        }
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