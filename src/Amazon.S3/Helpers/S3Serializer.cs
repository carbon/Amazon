﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3;

internal static class S3Serializer<T>
    where T : class
{
    private static readonly XmlSerializer serializer = new(typeof(T));

    public static T Deserialize(string xmlText)
    {
        using var reader = new StringReader(xmlText);

        return (T)serializer.Deserialize(reader)!;
    }

    public static bool TryDeserialize(string xmlText, [NotNullWhen(true)] out T? result)
    {
        try
        {
            using var reader = new StringReader(xmlText);

            result = (T)serializer.Deserialize(reader)!;

            return true;
        }
        catch
        {
            result = null;

            return false;
        }
    }
}