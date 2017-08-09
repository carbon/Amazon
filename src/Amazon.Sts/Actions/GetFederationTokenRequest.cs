namespace Amazon.Sts
{
    public class GetFederationTokenRequest : IStsRequest
    {
        public string Action => "GetFederationToken";


        public int DurationSeconds { get; set; }

        public string Name { get; set; }

        public string Policy { get; set; }
    }
}