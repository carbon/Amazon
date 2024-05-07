using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Amazon.Route53;

public static class Route53Serializer<T>
    where T : notnull
{
    private static readonly XmlSerializer s_serializer = new(typeof(T), Route53Client.Namespace);
    private static readonly UTF8Encoding _encoding = new(encoderShouldEmitUTF8Identifier: false);
    public static byte[] SerializeToUtf8Bytes(T instance)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, _encoding);

        s_serializer.Serialize(writer, instance, XmlSerializerNamespacesCache.Get());

        return stream.ToArray();
    }

    public static T DeserializeXml(string xmlText)
    {
        using var reader = new StringReader(xmlText);

        return (T)s_serializer.Deserialize(reader)!;
    }
}