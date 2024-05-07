using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ses;

internal static class SesSerializer<T>
{
    private static readonly XmlSerializer s_serializer = new(typeof(T));

    public static T Deserialize(byte[] xmlText)
    {
        using var reader = new MemoryStream(xmlText);

        return (T)s_serializer.Deserialize(reader)!;
    }
}