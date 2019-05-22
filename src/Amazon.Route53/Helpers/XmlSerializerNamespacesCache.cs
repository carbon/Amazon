using System.Xml.Serialization;

namespace Amazon.Route53
{
    internal static class XmlSerializerNamespacesCache
    {
        private static XmlSerializerNamespaces ns;

        public static XmlSerializerNamespaces Get()
        {
            if (ns is null)
            {
                var namespaces = new XmlSerializerNamespaces(); // TODO: Avoid this allocation

                namespaces.Add(string.Empty, Route53Client.Namespace);

                ns = namespaces;
            }

            return ns;
        }
    }
}