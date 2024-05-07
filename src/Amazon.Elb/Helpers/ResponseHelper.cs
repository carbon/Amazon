using System.IO;
using System.Xml.Serialization;

namespace Amazon.Elb;

public static class ElbSerializer<T>
{
    private static readonly XmlSerializer s_serializer = new(typeof(T), ElbClient.Namespace);

    public static T DeserializeXml(byte[] xmlText)
    {
        using var reader = new MemoryStream(xmlText);

        return (T)s_serializer.Deserialize(reader)!;
    }
}