namespace Amazon.Sts
{
    public class DecodeAuthorizationMessageRequest : IStsRequest
    {
        public string Action => "DecodeAuthorizationMessage";

        public string EncodedMessage { get; set; }
    }
}