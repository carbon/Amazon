namespace Amazon.Rekognition;

public sealed class Label
{
    public LabelAlias[]? Aliases { get; init; }

    public LabelCategory[]? Categories { get; init; }

    /// <summary>
    /// If Label represents an object, Instances contains the bounding boxes for each instance of the detected object. 
    /// Bounding boxes are returned for common object labels such as people, cars, furniture, apparel or pets.
    /// </summary>
    public Instance[]? Instances { get; init; }

    public double Confidence { get; init; }

    public required string Name { get; init; }

    public Parent[]? Parents { get; init; }
}