using Carbon.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.S3
{
    public class ListBucketResult : IReadOnlyList<IBlob>
    {
        private static readonly XNamespace ns = S3Client.NS;

        public string BucketName { get; set; }

        public string Marker { get; set; }

        public int MaxKeys { get; set; }

        public string Prefix { get; set; }

        public List<S3ObjectInfo> Items { get; } = new List<S3ObjectInfo>();

        public static ListBucketResult ParseXml(string xmlText)
        {
            var result = new ListBucketResult();

            var rootEl = XElement.Parse(xmlText);                   // <ListBucketResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">

            result.BucketName   = rootEl.Element(ns + "Name").Value;
            result.Prefix       = rootEl.Element(ns + "Prefix").Value;
            result.Marker       = rootEl.Element(ns + "Marker").Value;
            result.MaxKeys      = (int)rootEl.Element(ns + "MaxKeys");

            foreach (var el in rootEl.Elements(ns + "Contents"))
            {
                var a = new S3ObjectInfo(
                    name : el.Element(ns + "Key").Value,
                    size : (long)el.Element(ns + "Size")
                );

                result.Items.Add(a);
            }

            return result;
        }

        #region IReadOnlyCollection<IBlob>

        int IReadOnlyCollection<IBlob>.Count => Items.Count;

        IBlob IReadOnlyList<IBlob>.this[int index] => Items[index];

        #endregion

        #region IEnumerable<IBlobInfo>

        IEnumerator<IBlob> IEnumerable<IBlob>.GetEnumerator()
            => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Items.GetEnumerator();

        #endregion
    }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<ListBucketResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
	<Name>cmcdn</Name>
	<Prefix>1</Prefix>
	<Marker></Marker>
	<MaxKeys>100</MaxKeys>
	<IsTruncated>true</IsTruncated>
	<Contents>
		<Key>100000/800x600.jpeg</Key>
		<LastModified>2009-06-20T09:54:05.000Z</LastModified>
		<ETag>&quot;c55fad5b272947050bed993ec22c6d0d&quot;</ETag>
		<Size>116879</Size>
		<Owner>
			<ID>9c18bda0312b59b259789b4acf29a06efdb6193a4ef51fcafa739f5cda4f3bf0</ID>
			<DisplayName>jason17095</DisplayName>
		</Owner>
		<StorageClass>STANDARD</StorageClass>
	</Contents>
	<Contents>
		<Key>100001/800x600.jpeg</Key>
		<LastModified>2009-06-20T09:54:01.000Z</LastModified>
		<ETag>&quot;4ef58e19a01ea04d4f9da27c6f6638d7&quot;</ETag>
		<Size>116882</Size>
		<Owner>
			<ID>9c18bda0312b59b259789b4acf29a06efdb6193a4ef51fcafa739f5cda4f3bf0</ID>
			<DisplayName>jason17095</DisplayName>
		</Owner>
		<StorageClass>STANDARD</StorageClass>
	</Contents>
</ListBucketResult>
*/
