using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.CloudFront;

public sealed class InvalidationBatch
{
	public InvalidationBatch(IList<string> paths)
	{
		ArgumentNullException.ThrowIfNull(paths);

		if (paths.Count == 0) 
			throw new ArgumentException("May not be empty", nameof(paths));

		Paths = paths;
	}

	public IList<string> Paths { get; }

	public string CallerReference { get; set; }

	public XElement ToXml()
	{
		var root = new XElement("InvalidationBatch");

		foreach (var path in Paths)
		{
			root.Add(new XElement("Path", path));
		}

		root.Add(new XElement("CallerReference", CallerReference));

		return root;
	}
}

/*
<InvalidationBatch>
   <Path>/image1.jpg</Path>
   <Path>/image2.jpg</Path>
   <Path>/videos/movie.flv</Path>
   <Path>/sound%20track.mp3</Path>				
   <CallerReference>my-batch</CallerReference>
</InvalidationBatch>
*/