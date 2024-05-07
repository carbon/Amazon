using System.IO;
using System.Text;
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

internal static class CloudFrontSerializerOptions
{
    public static readonly XmlWriterSettings Settings = new() {
        Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
        Indent = true
    };

    private static XmlSerializerNamespaces? s_instance = null;

    public static XmlSerializerNamespaces GetNamespaces()
    {
        if (s_instance is null)
        {
            var ns = new XmlSerializerNamespaces();

            ns.Add("", "");

            s_instance = ns;

            return ns;
        }

        return s_instance;
    }    
}