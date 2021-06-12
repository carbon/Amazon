#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class Matcher
    {
        public Matcher() { }

        public Matcher(string httpCode)
        {
            HttpCode = httpCode;
        }

        [XmlElement]
        public string HttpCode { get; init; }
    }
}

// The HTTP codes. You can specify values between 200 and 499. The default value is 200.
// You can specify multiple values (for example, "200,202") or a range of values (for example, "200-299").

