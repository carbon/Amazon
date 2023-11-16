namespace Amazon.Rekognition;

public sealed class PersonDetail
{
    public BoundingBox BoundingBox { get; init; }

    public FaceDetail? Face { get; init; }

    public long Index { get; init; }
}