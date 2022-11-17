using System.IO;
using System.Xml.Serialization;

namespace Amazon.Sts.Serialization;

public static class StsXmlSerializer<T>
{
    private static readonly XmlSerializer serializer = new(typeof(T), StsClient.Namespace);

    public static T Deserialize(string xmlText)
    {
        using var reader = new StringReader(xmlText);

        return (T)serializer.Deserialize(reader)!;
    }
}