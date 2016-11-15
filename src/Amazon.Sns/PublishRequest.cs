using System.Collections.Generic;

namespace Amazon.Sns
{
    public class PublishRequest
    {
        public string Subject { get; set; }

        // [Required]
        public string TopicArn { get; set; }

        // [Required]
        public string Message { get; set; }

        public Dictionary<string, string> ToParams()
        {
            var dic = new Dictionary<string, string> {
                { "Action", "Publish" },
                { "TopicArn", TopicArn },
                { "Message", Message }
            };

            if (Subject != null)
            {
                dic.Add(Subject, Subject);
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
