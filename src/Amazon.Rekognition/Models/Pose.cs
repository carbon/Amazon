namespace Amazon.Rekognition;

public sealed class Pose
{
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