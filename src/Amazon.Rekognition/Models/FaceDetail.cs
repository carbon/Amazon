namespace Amazon.Rekognition;

public class FaceDetail
{
    public AgeRange? AgeRange { get; set; }

    public Beard? Beard { get; set; }

    public BoundingBox BoundingBox { get; set; }

    public List<Landmark>? Landmarks { get; set; }

    public float Confidence { get; set; }

    public Emotion[]? Emotions { get; set; }

    public Eyeglasses? Eyeglasses { get; set; }

    public Sunglasses? Sunglasses { get; set; }

    public Smile? Smile { get; set; }

    public Pose? Pose { get; set; }

    public ImageQuality? Quality { get; set; }
}
