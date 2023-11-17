#nullable disable

namespace Amazon.Rekognition;

public readonly struct DominantColor
{
    public int Red { get; init; }

    public int Green { get; init; }

    public int Blue { get; init; }

    public string CSSColor { get; init; }

    public string HexCode { get; init; }

    public double PixelPercent { get; init; }

    public string SimplifiedColor { get; init; }

    internal (int, int, int) GetRGB() => (Red, Green, Blue);
}
