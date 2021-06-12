#nullable disable

using System;

namespace Amazon.Elb
{
    public sealed class Certificate
    {
        public Certificate() { }

        public Certificate(string arn)
        {
            CertificateArn = arn ?? throw new ArgumentNullException(nameof(arn));
        }

        public string CertificateArn { get; set; }
    }
}
