namespace Amazon.Sns;

public sealed class ConfirmSubscriptionRequest
{
    public ConfirmSubscriptionRequest(string topicArn, string token, bool authenticateOnUnsubscribe = false)
    {
        ArgumentException.ThrowIfNullOrEmpty(topicArn);
        ArgumentException.ThrowIfNullOrEmpty(token);

        TopicArn = topicArn;
        Token = token;
        AuthenticateOnUnsubscribe = authenticateOnUnsubscribe;
    }

    public string TopicArn { get; }

    public string Token { get; }

    public bool AuthenticateOnUnsubscribe { get; }

    internal List<KeyValuePair<string, string>> ToParams()
    {
        var parameters = new List<KeyValuePair<string, string>>(5) {
            new ("Action", "ConfirmSubscription"),
            new ("TopicArn", TopicArn),
            new ("Token", Token)
        };

        if (AuthenticateOnUnsubscribe)
        {
            parameters.Add(new ("AuthenticateOnUnsubscribe", "true"));
        }

        return parameters;
    }
}

// https://docs.aws.amazon.com/sns/latest/api/API_ConfirmSubscription.html
