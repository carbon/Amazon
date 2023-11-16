namespace Amazon.Rekognition;

public sealed class Instance
{
    public BoundingBox BoundingBox { get; init; }

    public double Confidence { get; init; }

    public DominantColor[]? DominantColors {get; init; }
}