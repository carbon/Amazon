﻿using System.IO;
using System.Xml.Serialization;

namespace Amazon.Elb;

public static class ElbSerializer<T>
{
    private static readonly XmlSerializer s_serializer = new(typeof(T), ElbClient.Namespace);

    public static T DeserializeXml(string xmlText)
    {
        using var reader = new StringReader(xmlText);

        return (T)s_serializer.Deserialize(reader)!;
    }
}