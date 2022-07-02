using System.Globalization;

namespace Amazon.Sqs;

public sealed class SendMessageRequest
{
    public SendMessageRequest(string body)
    {
        ArgumentNullException.ThrowIfNull(body);

        MessageBody = body;
    }

    public SendMessageRequest(string body, string messageDeduplicationId, string messageGroupId)
    {
        ArgumentNullException.ThrowIfNull(body);

        MessageBody = body;
        MessageDeduplicationId = messageDeduplicationId;
        MessageGroupId = messageGroupId;
    }

    public string MessageBody { get; }

    // 128 characters
    public string? MessageDeduplicationId { get; }

    // Required for FIFO queues
    public string? MessageGroupId { get; }

    /// <summary>
    /// The number of seconds (0 to 900 - 15 minutes) to delay a specific message. 
    /// Messages with a positive DelaySeconds value become available for processing after the delay time is finished. 
    /// If you don't specify a value, the default value for the queue applies.
    /// </summary>
    public TimeSpan? Delay { get; set; }

    public MessageAttribute[]? MessageAttributes { get; set; }

    internal List<KeyValuePair<string, string>> GetParameters()
    {
        var parameters = new List<KeyValuePair<string, string>>(4) {
            new ("Action", "SendMessage"),
            new ("MessageBody", MessageBody)
        };

        // Defaults to the queue visibility timeout
        if (Delay.HasValue)
        {
            int delaySeconds = (int)Delay.Value.TotalSeconds;

            parameters.Add(new("DelaySeconds", delaySeconds.ToString(CultureInfo.InvariantCulture)));
        }

        if (MessageDeduplicationId is not null)
        {
            parameters.Add(new("MessageDeduplicationId", MessageDeduplicationId));
        }

        if (MessageGroupId is not null)
        {
            parameters.Add(new("MessageGroupId", MessageGroupId));
        }

        if (MessageAttributes is { Length: > 0 })
        {
            for (int i = 0; i < MessageAttributes.Length; i++)
            {
                int number = i + 1;

                ref MessageAttribute attr = ref MessageAttributes[i];

                string prefix = string.Create(CultureInfo.InvariantCulture, $"MessageAttribute.{number}.");

                parameters.Add(new(prefix + "Name", attr.Name));

                if (attr.Value.DataType is MessageAttributeDataType.Binary)
                {
                    parameters.Add(new(prefix + "Value.BinaryValue", attr.Value.Value));
                }
                else
                {
                    parameters.Add(new(prefix + "Value.StringValue", attr.Value.Value));
                }

                parameters.Add(new(prefix + "Value.DataType", attr.Value.DataType.Canonicalize()));
            }
        }

        return parameters;
    }
}