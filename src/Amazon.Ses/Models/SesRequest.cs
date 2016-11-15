namespace Amazon.Ses
{
    public class SesRequest : AwsRequest
    {
        public SesRequest(string action)
        {
            Parameters.Add("Action", action);
        }
    }
}