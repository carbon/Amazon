using System;

namespace Amazon.Sts;

public sealed class DecodeAuthorizationMessageRequest : IStsRequest
{
    public string Action => "DecodeAuthorizationMessage";

    public DecodeAuthorizationMessageRequest(string encodedMessage)
    {
        ArgumentNullException.ThrowIfNull(encodedMessage);

        EncodedMessage = encodedMessage;
    }

    public string EncodedMessage { get; }
}