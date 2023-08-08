using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public sealed class Pose(
    double pitch,
    double roll,
    double yaw)
{
    public double Pitch { get; } = pitch;

    public double Roll { get; } = roll;

    public double Yaw { get; } = yaw;
}