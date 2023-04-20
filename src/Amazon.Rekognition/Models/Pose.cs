using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class Pose
{
    [JsonConstructor]
    public Pose(double pitch, double roll, double yaw)
    {
        Pitch = pitch;
        Roll = roll;
        Yaw = yaw;
    }

    public double Pitch { get; }

    public double Roll { get; }

    public double Yaw { get; }
}