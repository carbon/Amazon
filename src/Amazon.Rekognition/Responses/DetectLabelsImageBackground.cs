namespace Amazon.Rekognition;

public sealed class DetectLabelsImageBackground
{
    public DetectLabelsImageBackground(
        DominantColor[] dominantColors,
        DetectLabelsImageQuality quality)
    {
        DominantColors = dominantColors;
        Quality = quality;
    }

    public DominantColor[] DominantColors { get; }

    public DetectLabelsImageQuality Quality { get; }
}
