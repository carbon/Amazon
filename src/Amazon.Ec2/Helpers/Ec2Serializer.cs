using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ec2;

public static class Ec2Serializer<T>
{
    private static readonly XmlSerializer serializer = new(typeof(T), Ec2Client.Namespace);

    public static T Deserialize(byte[] xmlText)
    {
        using var reader = new MemoryStream(xmlText);

        return (T)serializer.Deserialize(reader)!;
    }
}