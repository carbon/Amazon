namespace Amazon.Sts
{
    public class GetCallerIdentityRequest : IStsRequest
    {
        public static readonly GetCallerIdentityRequest Default = new GetCallerIdentityRequest();

        public string Action => "GetCallerIdentity";
    }
}