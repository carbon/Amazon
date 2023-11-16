namespace Amazon.Rekognition;

public class FaceDetail
{
    public AgeRange? AgeRange { get; init; }

    public Beard? Beard { get; init; }

    public BoundingBox BoundingBox { get; init; }

    public List<Landmark>? Landmarks { get; init; }

    public float Confidence { get; init; }

    public Emotion[]? Emotions { get; init; }

    public Eyeglasses? Eyeglasses { get; init; }

    public Sunglasses? Sunglasses { get; init; }

    public Smile? Smile { get; init; }

    public Pose? Pose { get; init; }

    public ImageQuality? Quality { get; init; }
}
