namespace Amazon.CloudWatch
{
    public class ListMetricsRequest
    {     
        public string MetricName { get; set; }

        public string Namespace { get; set; }

        public string NextToken { get; set; }

        public AwsRequest ToParams()
        {
            var parameters = new AwsRequest {
                { "Action", "ListMetrics" }
            };

            if (MetricName != null) parameters.Add("MetricName", MetricName);
            if (Namespace != null)  parameters.Add("Namespace", Namespace);
            if (NextToken != null)  parameters.Add("NextToken", NextToken);

            return parameters;
        }
    }
}
