#nullable disable

namespace Amazon.CloudWatch;

public class AlarmHistoryItem
{
    public string AlarmName { get; set; }

    public string HistoryData { get; set; }

    public string HistoryItemType { get; set; }

    public string HistorySummary { get; set; }

    public string Timestamp { get; set; }
}
