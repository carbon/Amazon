using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Amazon.CloudFront.Serialization;

internal static class CloudFrontSerializerOptions
{
    public static readonly XmlWriterSettings Settings = new() {
        Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
        Indent = true
    };

    private static XmlSerializerNamespaces? s_instance = null;

    public static XmlSerializerNamespaces GetNamespaces()
    {
        if (s_instance is null)
        {
            var ns = new XmlSerializerNamespaces();

            ns.Add("", "");

            s_instance = ns;

            return ns;
        }

        return s_instance;
    }    
}