namespace Amazon.Rekognition;

public sealed class DetectLabelsRequest
{
    public Feature[]? Features { get; set; }

    public required Image Image { get; set; }

    public int? MaxLabels { get; set; }

    public double? MinConfidence { get; set; }

    public DetectLabelSettings? Settings { get; set; }
}