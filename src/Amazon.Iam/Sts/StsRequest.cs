namespace Amazon.Sts;

public sealed class StsRequest : AwsRequest
{
    public StsRequest(string action)
    {
        Parameters.Add(new("Action", action));
    }
}