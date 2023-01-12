using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Amazon.Sqs;

public sealed class ChangeMessageVisibilityRequest
{
    public ChangeMessageVisibilityRequest(string receiptHandle, TimeSpan visibilityTimeout)
    {
        ArgumentException.ThrowIfNullOrEmpty(receiptHandle);

        if (visibilityTimeout < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(visibilityTimeout), visibilityTimeout, "Must be greater than 0");
        }

        if (visibilityTimeout > TimeSpan.FromHours(12))
        {
            throw new ArgumentOutOfRangeException(nameof(visibilityTimeout), visibilityTimeout, "Must be less than 12 hours");
        }

        ReceiptHandle = receiptHandle;
        VisibilityTimeout = (int)visibilityTimeout.TotalSeconds;
    }

    public string ReceiptHandle { get; }
        
    [Range(0, 43_200)]
    public int VisibilityTimeout { get; }

    internal List<KeyValuePair<string, string>> ToParams()
    {
        return new(4) {
            new ("Action",            "ChangeMessageVisibility"),
            new ("ReceiptHandle",     ReceiptHandle),
            new ("VisibilityTimeout", VisibilityTimeout.ToString(CultureInfo.InvariantCulture))
        };
    }
}