using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[method: JsonConstructor]
public readonly struct BoundingBox(double height, double width, double top, double left)
{
    public double Height { get; } = height;

    public double Width { get; } = width;

    public double Top { get; } = top;

    public double Left { get; } = left;
}
