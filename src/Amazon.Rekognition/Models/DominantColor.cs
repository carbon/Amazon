#nullable disable

namespace Amazon.Rekognition;

public struct DominantColor
{
    public int Red { get; set; }

    public int Green { get; set; }

    public int Blue { get; set; }

    public string CSSColor { get; set; }

    public string HexCode { get; set; }

    public double PixelPercent { get; set; }

    public string SimplifiedColor { get; set; }

    internal (int, int, int) GetRGB() => (Red, Green, Blue);
}
