#nullable enable

using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public static class Ec2ResponseHelper<T>
        where T: IEc2Response
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T), Ec2Client.Namespace);

        public static T ParseXml(string xml)
        {
            using var reader = new StringReader(xml);

            return (T)serializer.Deserialize(reader);
        }
    }
}
