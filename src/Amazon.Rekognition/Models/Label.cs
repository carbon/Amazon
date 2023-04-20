namespace Amazon.Rekognition;

public sealed class Label
{
    public LabelAlias[]? Aliases { get; set; }

    public LabelCategory[]? Categories { get; set; }

    /// <summary>
    /// If Label represents an object, Instances contains the bounding boxes for each instance of the detected object. 
    /// Bounding boxes are returned for common object labels such as people, cars, furniture, apparel or pets.
    /// </summary>
    public Instance[]? Instances { get; set; }

    public double Confidence { get; set; }

    public string Name { get; set; }

    public Parent[]? Parents { get; set; }
}