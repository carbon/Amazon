using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3
{
    internal static class ResponseHelper<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T));

        public static T ParseXml(string xml)
        {            
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}