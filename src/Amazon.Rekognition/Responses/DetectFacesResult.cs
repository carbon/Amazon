namespace Amazon.Rekognition;

public sealed class DetectFacesResult
{
    public required FaceDetail FaceDetails { get; set; }

    public OrientationCorrection OrientationCorrection { get; set; }
}