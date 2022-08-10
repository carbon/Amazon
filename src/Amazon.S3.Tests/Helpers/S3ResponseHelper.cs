using System.Xml.Serialization;

namespace Amazon.S3.Models.Tests;

internal static class S3Serializer<T>
{
    private static readonly XmlSerializer serializer = new(typeof(T));

    public static T Deserialize(string xml)
    {
        using var reader = new StringReader(xml);

        return (T)serializer.Deserialize(reader);
    }
}