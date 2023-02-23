namespace Amazon.Rekognition;

public sealed class DetectFacesResult
{
    public FaceDetail FaceDetails { get; set; }

    public OrientationCorrection OrientationCorrection { get; set; }
}