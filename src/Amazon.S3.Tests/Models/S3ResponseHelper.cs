using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3.Models.Tests
{
    internal static class S3ResponseHelper<T>
	{
		private static readonly XmlSerializer serializer = new (typeof(T));

		public static T ParseXml(string xml)
		{
			using var reader = new StringReader(xml);

			return (T)serializer.Deserialize(reader);
		}
	}
}