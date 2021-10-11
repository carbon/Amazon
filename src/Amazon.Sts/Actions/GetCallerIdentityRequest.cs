namespace Amazon.Sts;

public sealed class GetCallerIdentityRequest : IStsRequest
{
    public static readonly GetCallerIdentityRequest Default = new ();

    public string Action => "GetCallerIdentity";

    public GetCallerIdentityRequest() { }
}