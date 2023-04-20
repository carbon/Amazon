namespace Amazon.Rekognition;

public sealed class DetectLabelsResult
{
    public DetectLabelsImageProperties? ImageProperties { get; set; }

    public required string LabelModelVersion { get; set; }

    /// <summary>
    /// An array of labels for the real-world objects detected.
    /// </summary>
    public required Label[] Labels { get; set; }

    public OrientationCorrection OrientationCorrection { get; set; }
}