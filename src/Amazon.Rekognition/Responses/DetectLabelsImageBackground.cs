namespace Amazon.Rekognition;

public sealed class DetectLabelsImageBackground(
    DominantColor[] dominantColors,
    DetectLabelsImageQuality quality)
{
    public DominantColor[] DominantColors { get; } = dominantColors;

    public DetectLabelsImageQuality Quality { get; } = quality;
}
