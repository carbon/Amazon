using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Amazon.Sns;

internal static class SnsXml
{
    public static XElement Parse(byte[] utf8Xml)
    {
        using var stream = new MemoryStream(utf8Xml);

        return XDocument.Load(stream).Root!;
    }

    public static string? GetValue(XElement parent, string localName)
    {
        return parent.Descendants().FirstOrDefault(element => element.Name.LocalName == localName)?.Value;
    }

    public static string GetRequiredValue(byte[] utf8Xml, string localName)
    {
        return GetValue(Parse(utf8Xml), localName) ?? throw new FormatException($"The response is missing {localName}");
    }
}
