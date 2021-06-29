#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot("ErrorResponse", Namespace = "https://iam.amazonaws.com/doc/2010-05-08/")]
    public sealed class S3ErrorResponse
    {
        public S3Error Error { get; init; }
    }
}

// Wasabi returns a non-compliant exception under the iam namespace