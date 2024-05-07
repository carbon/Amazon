#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3;

[XmlRoot(Namespace = S3Client.Namespace)]
public sealed class CompleteMultipartUploadResult
{
    [XmlElement]
    public string Location { get; init; }

    [XmlElement]
    public string Bucket { get; init; }

    [XmlElement]
    public required string Key { get; init; }

    /// <summary>
    /// The entity tag is an opaque string. The entity tag may or may not be an MD5 digest of the object data.
    /// </summary>
    [XmlElement]
    public required string ETag { get; init; }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<CompleteMultipartUploadResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
  <Location>http://Example-Bucket.s3.amazonaws.com/Example-Object</Location>
  <Bucket>Example-Bucket</Bucket>
  <Key>Example-Object</Key>
  <ETag>"3858f62230ac3c915f300c664312c11f-9"</ETag>
</CompleteMultipartUploadResult>
*/
