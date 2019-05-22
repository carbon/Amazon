namespace Amazon.Route53
{
    public sealed class GetChangeRequest
    {
        public GetChangeRequest(string id)
        {
            Id = id;
        }

        // Max Length = 32
        public string Id { get; }
    }
}
