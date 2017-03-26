using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Carbon.Storage;

namespace Amazon.S3
{
    [XmlRoot("Contents", Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public class ListBucketObject : IBlob
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("LastModified", DataType = "dateTime")]
        public DateTime LastModified { get; set; }

        [XmlElement("ETag")]
        public string ETag { get; set; }

        [XmlElement("Size")]
        public long Size { get; set; }

        [XmlElement("StorageClass")]
        public string StorageClass { get; set; }

        #region IBlob

        string IBlob.Name => Key;

        DateTime IBlob.Modified => LastModified;

        IReadOnlyDictionary<string, string> IBlob.Metadata => null;

        ValueTask<Stream> IBlob.OpenAsync()
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
        }

        #endregion


    }
}

/*
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
*/