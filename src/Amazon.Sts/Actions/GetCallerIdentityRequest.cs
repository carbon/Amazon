namespace Amazon.Sts;

public sealed class GetCallerIdentityRequest : IStsRequest
{
    public static readonly GetCallerIdentityRequest Default = new ();

    public GetCallerIdentityRequest() { }

    string IStsRequest.Action => "GetCallerIdentity";
}