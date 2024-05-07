using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3;

internal static class S3Serializer<T>
    where T : class
{
    private static readonly XmlSerializer s_serializer = new(typeof(T));

    public static T Deserialize(byte[] xmlText)
    {
        using var stream = new MemoryStream(xmlText);

        return (T)s_serializer.Deserialize(stream)!;
    }

    public static bool TryDeserialize(byte[] xmlText, [NotNullWhen(true)] out T? result)
    {
        using var reader = new MemoryStream(xmlText);

        try
        {
            result = (T)s_serializer.Deserialize(reader)!;

            return true;
        }
        catch
        {
            result = null;

            return false;
        }
    }
}