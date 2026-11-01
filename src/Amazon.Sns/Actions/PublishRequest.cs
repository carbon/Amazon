namespace Amazon.Sns;

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

    // Set to "json" to send a different message per protocol
    public string? MessageStructure { get; init; }

    public IReadOnlyDictionary<string, MessageAttributeValue>? MessageAttributes { get; init; }

    public string? MessageGroupId { get; init; }

    public string? MessageDeduplicationId { get; init; }

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

        if (MessageStructure is not null)
        {
            dic.Add(new ("MessageStructure", MessageStructure));
        }

        if (MessageGroupId is not null)
        {
            dic.Add(new ("MessageGroupId", MessageGroupId));
        }

        if (MessageDeduplicationId is not null)
        {
            dic.Add(new ("MessageDeduplicationId", MessageDeduplicationId));
        }

        if (MessageAttributes is not null)
        {
            int i = 1;

            foreach (var (name, value) in MessageAttributes)
            {
                string prefix = $"MessageAttributes.entry.{i}";

                dic.Add(new ($"{prefix}.Name", name));
                dic.Add(new ($"{prefix}.Value.DataType", value.DataType));

                if (value.StringValue is not null)
                {
                    dic.Add(new ($"{prefix}.Value.StringValue", value.StringValue));
                }

                if (value.BinaryValue is not null)
                {
                    dic.Add(new ($"{prefix}.Value.BinaryValue", Convert.ToBase64String(value.BinaryValue)));
                }

                i++;
            }
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