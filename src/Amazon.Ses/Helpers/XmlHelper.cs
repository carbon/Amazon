using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ses
{
    internal static class XmlHelper<T>
    {
        private static readonly XmlSerializer serializer = new (typeof(T));

        public static T Deserialize(string xmlText)
        {
            using var reader = new StringReader(xmlText);

            return (T)serializer.Deserialize(reader)!;
        }
    }
}