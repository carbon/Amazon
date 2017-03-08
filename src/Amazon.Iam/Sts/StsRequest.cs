namespace Amazon.Sts
{
    public class StsRequest : AwsRequest
    {
        public StsRequest(string action)
        {
            Parameters.Add("Action", action);
        }
    }
}