using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public readonly struct Landmark(
    string type,
    double x,
    double y)
{
    // eyeLeft eyeRight | nose | mouthLeft | mouthRight | leftEyeBrowLeft | leftEyeBrowRight | leftEyeBrowUp | rightEyeBrowLeft | rightEyeBrowRight | rightEyeBrowUp | leftEyeLeft | leftEyeRight | leftEyeUp | leftEyeDown | rightEyeLeft | rightEyeRight | rightEyeUp | rightEyeDown | noseLeft | noseRight | mouthUp | mouthDown | leftPupil | rightPupil | upperJawlineLeft | midJawlineLeft | chinBottom | midJawlineRight | upperJawlineRight

    public string Type { get; } = type;

    /// <summary>
    /// The x-coordinate of the landmark expressed as a ratio of the width of the image. 
    /// The x-coordinate is measured from the left-side of the image. 
    /// </summary>
    public double X { get; } = x;

    /// <summary>
    /// The y-coordinate of the landmark expressed as a ratio of the height of the image.
    /// The y-coordinate is measured from the top of the image.
    /// </summary>
    public double Y { get; } = y;
}