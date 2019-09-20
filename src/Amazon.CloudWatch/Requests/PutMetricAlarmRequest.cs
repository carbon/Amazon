#nullable disable

using System.Collections.ObjectModel;

namespace Amazon.CloudWatch
{
    public class PutMetricAlarmRequest 
    {     
        public string Namespace { get; set; }
        
        public bool ActionsEnabled { get; set; }
        
        public AwsRequest ToParams()
        {
            var parameters = new AwsRequest {
                { "Action", "PutMetricAlarm" }
            };

            // TODO

            return parameters;
        }
    }
}
