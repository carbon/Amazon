using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public readonly struct BoundingBox
{
    [JsonConstructor]
    public BoundingBox(double height, double width, double top, double left)
    {
        Height = height;
        Width = width;
        Top = top;
        Left = left;
    }

    public double Height { get; }

    public double Width { get; }

    public double Top { get; }

    public double Left { get; }
}
