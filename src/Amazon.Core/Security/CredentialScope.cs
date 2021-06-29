using System;
using System.Globalization;
using System.Text;

namespace Amazon.Security
{
    public readonly struct CredentialScope
    {
        public CredentialScope(DateTime date, AwsRegion region, AwsService service)
        {
            Date    = date;
            Region  = region ?? throw new ArgumentNullException(nameof(region));
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public DateTime Date { get; }

        public AwsRegion Region { get; }

        public AwsService Service { get; }

        // 20120228/us-east-1/iam/aws4_request
        public readonly override string ToString() => $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request";

        internal readonly void AppendTo(ref ValueStringBuilder output)
        {
            output.Append(Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            output.Append('/');
            output.Append(Region.Name);
            output.Append('/');
            output.Append(Service.Name);
            output.Append('/');
            output.Append("aws4_request");
        }
    }
}