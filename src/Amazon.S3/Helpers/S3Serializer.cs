using System;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3
{
    internal static class ResponseHelper<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T), S3Client.NS.ToString());

        public static T ParseXml(string xml)
        {
            #region Preconditions

            if (xml == null)
                throw new ArgumentNullException(nameof(xml));
            
            #endregion

            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}