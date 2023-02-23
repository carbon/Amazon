namespace Amazon.Rekognition;

public sealed class DetectLabelsImagePropertiesSettings
{
    public DetectLabelsImagePropertiesSettings(int maxDominantColors)
    {
        if (maxDominantColors > 20)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDominantColors), "Must be 20 or fewer");
        }

        MaxDominantColors = maxDominantColors;
    }

    public int MaxDominantColors { get; }
}
