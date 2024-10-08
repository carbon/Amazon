using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Amazon.CloudFront.Serialization;

public static class CloudFrontSerializer<T>
    where T : notnull
{
    private static readonly XmlSerializer s_serializer = new(typeof(T), CloudFrontClient.Namespace);

    public static byte[] SerializeToUtf8Bytes(T instance)
    {
        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, CloudFrontSerializerOptions.Settings);

        s_serializer.Serialize(writer, instance, CloudFrontSerializerOptions.GetNamespaces());

        return stream.ToArray();
    }

    public static T DeserializeXml(byte[] xmlText)
    {
        using var stream = new MemoryStream(xmlText);

        return (T)s_serializer.Deserialize(stream)!;
    }
}