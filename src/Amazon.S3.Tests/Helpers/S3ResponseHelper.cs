using System.Xml.Serialization;

namespace Amazon.S3.Models.Tests;

internal static class S3Serializer<T>
{
    private static readonly XmlSerializer s_serializer = new(typeof(T));

    public static T Deserialize(byte[] xml)
    {
        using var reader = new MemoryStream(xml);

        return (T)s_serializer.Deserialize(reader)!;
    }
}