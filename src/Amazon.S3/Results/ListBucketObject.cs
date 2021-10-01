#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Carbon.Storage;

namespace Amazon.S3;

[XmlRoot("Contents", Namespace = S3Client.Namespace)]
public sealed class ListBucketObject : IBlob
{
    [XmlElement("Key")]
    public string Key { get; init; }

    [XmlElement("LastModified", DataType = "dateTime")]
    public DateTime LastModified { get; init; }

    [XmlElement("ETag")]
    public string ETag { get; init; }

    [XmlElement("Size")]
    public long Size { get; init; }

    [XmlElement("StorageClass")]
    public string StorageClass { get; init; }

    [XmlElement("Owner")]
    public Owner Owner { get; init; }

    #region IBlob

    DateTime IBlob.Modified => LastModified;

    IReadOnlyDictionary<string, string> IBlob.Properties => BlobProperties.Empty;

    ValueTask<Stream> IBlob.OpenAsync()
    {
        throw new NotImplementedException();
    }

    void IDisposable.Dispose()
    {
    }

    #endregion
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