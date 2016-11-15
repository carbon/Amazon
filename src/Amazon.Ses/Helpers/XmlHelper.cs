using System.IO;
using System.Xml.Serialization;

namespace Amazon.Helpers
{
    public static class XmlText
    {
        public static T ToObject<T>(string text)
        {
            using (var reader = new StringReader(text))
            {
                var serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(reader);
            }
        }
    }
}