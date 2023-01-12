namespace Amazon.Sts;

public sealed class DecodeAuthorizationMessageRequest : IStsRequest
{
    string IStsRequest.Action => "DecodeAuthorizationMessage";

    public DecodeAuthorizationMessageRequest(string encodedMessage)
    {
        ArgumentException.ThrowIfNullOrEmpty(encodedMessage);

        EncodedMessage = encodedMessage;
    }

    public string EncodedMessage { get; }
}