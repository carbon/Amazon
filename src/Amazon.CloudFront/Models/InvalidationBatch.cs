using System.Xml.Serialization;

namespace Amazon.CloudFront;

[XmlRoot(Namespace = CloudFrontClient.Namespace)]
public sealed class InvalidationBatch
{
	[XmlElement]
	public required Paths Paths { get; init; }

    /// <summary>
    /// A value that you specify to uniquely identify an invalidation request. 
    /// </summary>
    [XmlElement]
	public required string CallerReference { get; init; }
}

public sealed class Paths
{
    [XmlArray("Items")]
    [XmlArrayItem("Path")]
    public required string[] Items { get; init; }
        
    public required int Quantity { get; set; }
}

/*
<InvalidationBatch xmlns="http://cloudfront.amazonaws.com/doc/2020-05-31/">
<Paths>
    <Items>
        <Path>string</Path>
    </Items>
    <Quantity>integer</Quantity>
</Paths>
<CallerReference>caller-reference</CallerReference>
</InvalidationBatch>
*/