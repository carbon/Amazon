namespace Amazon.Sts
{
    public sealed class GetCallerIdentityRequest : IStsRequest
    {
        public static readonly GetCallerIdentityRequest Default = new GetCallerIdentityRequest();

        public string Action => "GetCallerIdentity";

        public GetCallerIdentityRequest() { }
    }
}