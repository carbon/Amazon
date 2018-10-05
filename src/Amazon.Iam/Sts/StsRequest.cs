namespace Amazon.Sts
{
    public sealed class StsRequest : AwsRequest
    {
        public StsRequest(string action)
        {
            Parameters.Add("Action", action);
        }
    }
}