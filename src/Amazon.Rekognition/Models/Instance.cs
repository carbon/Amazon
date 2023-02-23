namespace Amazon.Rekognition;

public sealed class Instance
{
    public BoundingBox BoundingBox { get; set; }

    public double Confidence { get; set; }

    public DominantColor[]? DominantColors {get; set; }
}