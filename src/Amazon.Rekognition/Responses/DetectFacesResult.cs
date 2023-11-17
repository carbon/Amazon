namespace Amazon.Rekognition;

public sealed class DetectFacesResult
{
    public required FaceDetail FaceDetails { get; init; }

    public OrientationCorrection OrientationCorrection { get; init; }
}