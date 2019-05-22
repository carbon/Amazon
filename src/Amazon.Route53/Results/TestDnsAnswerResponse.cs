#nullable disable


namespace Amazon.Route53
{
    public class TestDnsAnswerResponse

    {
        public string Nameserver { get; set; }

        public string RecordName { get; set; }

        public string RecordType { get; set; }

        public RecordData RecordData { get; set; }

        public string ResponseCode { get; set; }

        public string Protocol { get; set; }

    }


    public class GetHealthCheckResponse
    {
        public HealthCheck HealthCheck { get; set; }
    }



}