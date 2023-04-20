namespace Amazon.Rekognition;

public sealed class DetectLabelsImageForeground
{
    public DetectLabelsImageForeground(
        DominantColor[] dominantColors,
        DetectLabelsImageQuality quality)
    {
        DominantColors = dominantColors;
        Quality = quality;
    }

    public DominantColor[] DominantColors { get; }

    public DetectLabelsImageQuality Quality { get; }
}