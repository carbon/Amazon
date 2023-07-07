namespace Amazon.Rekognition;

public sealed class DetectLabelsImageQuality(
    double brightness,
    double contrast,
    double sharpness)
{
    public double Brightness { get; } = brightness;

    public double Contrast { get; } = contrast;

    public double Sharpness { get; } = sharpness;
}