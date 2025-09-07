using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Amazon.Route53;

public static class Route53Serializer<T>
    where T : notnull
{
    private static readonly XmlSerializer s_serializer = new(typeof(T), Route53Client.Namespace);

    public static byte[] SerializeToUtf8Bytes(T instance)
    {
        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, RouteSerializerOptions.Settings);

        s_serializer.Serialize(writer, instance, RouteSerializerOptions.GetNamespaces());

        return stream.ToArray();
    }

    public static T DeserializeXml(byte[] xmlText)
    {
        using var stream = new MemoryStream(xmlText);

        return (T)s_serializer.Deserialize(stream)!;
    }
}


internal static class RouteSerializerOptions
{
    public static readonly XmlWriterSettings Settings = new() {
        Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
        NewLineChars = "\n",
        Indent = true
    };

    private static XmlSerializerNamespaces? s_instance = null;

    public static XmlSerializerNamespaces GetNamespaces()
    {
        if (s_instance is null)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, Route53Client.Namespace);

            s_instance = namespaces;

            return namespaces;
        }

        return s_instance;
    }
}