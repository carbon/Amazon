namespace Amazon.Sts
{
    public sealed class DecodeAuthorizationMessageRequest : IStsRequest
    {
        public string Action => "DecodeAuthorizationMessage";

        public DecodeAuthorizationMessageRequest(string encodedMessage)
        {
            EncodedMessage = encodedMessage;
        }

        public string EncodedMessage { get; }
    }
}