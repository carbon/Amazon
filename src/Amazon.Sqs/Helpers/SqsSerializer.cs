using System.IO;
using System.Xml.Serialization;

namespace Amazon.Sqs
{
    internal static class SqsSerializer<T>
    {
        private static readonly XmlSerializer serializer = new (typeof(T), SqsClient.NS);

        public static T Deserialize(string xmlText)
        {
            using var reader = new StringReader(xmlText);

            return (T)serializer.Deserialize(reader)!;
        }
    }
}
