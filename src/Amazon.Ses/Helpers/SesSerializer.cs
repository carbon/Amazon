using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ses;

internal static class SesSerializer<T>
{
    private static readonly XmlSerializer s_serializer = new(typeof(T));

    public static T Deserialize(string xmlText)
    {
        using var reader = new StringReader(xmlText);

        return (T)s_serializer.Deserialize(reader)!;
    }
}