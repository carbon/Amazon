namespace Amazon.Rekognition;

public sealed class DetectLabelsImageQuality
{
    public DetectLabelsImageQuality(
        double brightness,
        double contrast,
        double sharpness)
    {
        Brightness = brightness;
        Contrast = contrast;
        Sharpness = sharpness;
    }

    public double Brightness { get; }

    public double Contrast { get; }

    public double Sharpness { get; }
}