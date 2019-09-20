using System.IO;
using System.Xml.Serialization;

namespace Amazon.Sqs
{
    internal static class SqsSerializer<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T), SqsClient.NS);

        public static T Deserialize(string xml)
        {
            using var reader = new StringReader(xml);

            return (T)serializer.Deserialize(reader);
        }
    }
}
