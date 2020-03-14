using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ses
{
    internal static class XmlHelper<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T));

        public static T Deserialize(string text)
        {
            using var reader = new StringReader(text);

            return (T)serializer.Deserialize(reader);
        }
    }
}