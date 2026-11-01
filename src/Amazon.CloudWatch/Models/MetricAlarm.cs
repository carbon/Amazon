#nullable disable

using System.Xml.Serialization;

namespace Amazon.CloudWatch;

public class MetricAlarm
{
    public bool? ActionsEnabled { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<string> AlarmActions { get; set; }

    public string AlarmArn { get; set; }

    public DateTime? AlarmConfigurationUpdatedTimestamp { get; set; }

    public string AlarmDescription { get; set; }

    public string AlarmName { get; set; }

    public string ComparisonOperator { get; set; }

    public int? DatapointsToAlarm { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<Dimension> Dimensions { get; set; }

    public string EvaluateLowSampleCountPercentile { get; set; }

    public int? EvaluationPeriods { get; set; }

    public string EvaluationState { get; set; }

    public string ExtendedStatistic { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<string> InsufficientDataActions { get; set; }

    public string MetricName { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<MetricDataQuery> Metrics { get; set; }

    public string Namespace { get; set; }

    [XmlArray]
    [XmlArrayItem("member")]
    public List<string> OKActions { get; set; }

    public int? Period { get; set; }

    public string StateReason { get; set; }

    public string StateReasonData { get; set; }

    public DateTime? StateTransitionedTimestamp { get; set; }

    public DateTime? StateUpdatedTimestamp { get; set; }

    public string StateValue { get; set; }

    public string Statistic { get; set; }

    public double? Threshold { get; set; }

    public string ThresholdMetricId { get; set; }

    public string TreatMissingData { get; set; }

    public string Unit { get; set; }
}
