﻿using System.IO;
using System.Xml.Serialization;

namespace Amazon.Elb
{
    public static class ElbResponseHelper<T>
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(T), ElbClient.Namespace);

        public static T ParseXml(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
