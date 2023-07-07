namespace Amazon.Rekognition;

public sealed class DetectLabelsImageForeground(
    DominantColor[] dominantColors,
    DetectLabelsImageQuality quality)
{
    public DominantColor[] DominantColors { get; } = dominantColors;

    public DetectLabelsImageQuality Quality { get; } = quality;
}