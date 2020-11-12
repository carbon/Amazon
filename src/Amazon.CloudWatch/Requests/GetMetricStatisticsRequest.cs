#nullable disable

using System;
using System.Globalization;

namespace Amazon.CloudWatch
{
    public sealed class GetMetricStatisticsRequest
    {
        public GetMetricStatisticsRequest(string nameSpace, string metricName)
        {
            Namespace = nameSpace ?? throw new ArgumentNullException(nameof(nameSpace));
            MetricName = metricName ?? throw new ArgumentNullException(nameof(metricName));
        }
        
        // Required
        public string MetricName { get; }

        // Required
        public string Namespace { get; }

        public Dimension[] Dimensions { get; set; }

        public Statistic[] Statistics { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Unit { get; set; }

        /// <summary>
        /// The granularity, in seconds, of the returned data points. 
        /// A period can be as short as one minute (60 seconds) and must be a multiple of 60.
        /// The default value is 60.
        /// </summary>
        public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(60);

        public AwsRequest ToParams()
        {
            var parameters = new AwsRequest {
                { "Action"     , "GetMetricStatistics" },

                // Required paramaeters
                { "Namespace"  , Namespace },
                { "MetricName" , MetricName },
                { "StartTime"  , StartTime.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                { "EndTime"    , EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                { "Period"     , (int)Period.TotalSeconds }
            }; 
        
            if (Unit != null)
            {
                parameters.Add("Unit", Unit);
            }

            if (Dimensions != null)
            {
                for (int i = 0; i < Dimensions.Length; i++)
                {
                    var dimension = Dimensions[i];

                    string prefix = "Dimensions.member." + (i + 1).ToString(CultureInfo.InvariantCulture) + ".";

                    parameters.Add(prefix + "Name", dimension.Name);
                    parameters.Add(prefix + "Value", dimension.Value);
                }
            }

            if (Statistics != null)
            {
                for (int i = 0; i < Statistics.Length; i++)
                {
                    var stat = Statistics[i];

                    var prefix = "Statistics.member." + (i + 1);

                    parameters.Add(prefix, stat.Name);
                }
            }

            return parameters;
        }
    }
}