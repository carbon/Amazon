namespace Amazon.Ses;

public sealed class SesRequest : AwsRequest
{
    public SesRequest(string action)
    {
        Parameters.Add(new("Action", action));
    }
}