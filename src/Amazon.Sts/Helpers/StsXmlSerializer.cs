using System.IO;
using System.Xml.Serialization;

namespace Amazon.Sts.Serialization;

public static class StsXmlSerializer<T>
{
    private static readonly XmlSerializer serializer = new(typeof(T), StsClient.Namespace);

    public static T Deserialize(byte[] xmlText)
    {
        using var stream = new MemoryStream(xmlText);

        return (T)serializer.Deserialize(stream)!;
    }
}