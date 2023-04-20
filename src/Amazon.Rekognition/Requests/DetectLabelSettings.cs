namespace Amazon.Rekognition;

public sealed class DetectLabelSettings
{
    public GeneralLabelsSettings? GeneralLabels { get; set; }

    public DetectLabelsImagePropertiesSettings? ImageProperties { get; set; }
}
