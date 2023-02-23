namespace Amazon.Rekognition;

public sealed class PersonDetail
{
    public BoundingBox BoundingBox { get; set; }

    public FaceDetail Face { get; set; }

    public long Index { get; set; }
}
