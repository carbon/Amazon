using System.IO;
using System.Xml.Serialization;

namespace Amazon.Sts
{
    internal static class StsSerializer<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T), StsClient.Namespace);

        public static T ParseXml(string xml)
        {
            using var reader = new StringReader(xml);

            return (T)serializer.Deserialize(reader);
        }
    }
}