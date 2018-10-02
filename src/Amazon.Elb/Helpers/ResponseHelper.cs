using System;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.Elb
{
    public static class ElbResponseHelper<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T), ElbClient.Namespace);

        public static T DeserializeXml(string xml)
        {
            if (xml is null) throw new ArgumentNullException(nameof(xml));

            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
