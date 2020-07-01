using System;
using System.Globalization;
using System.IO;

namespace Amazon.Security
{
    public readonly struct CredentialScope
    {
        public CredentialScope(DateTime date, AwsRegion region, AwsService service)
        {
            Date    = date;
            Region  = region  ?? throw new ArgumentNullException(nameof(region));
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public readonly DateTime Date;

        public readonly AwsRegion Region;

        public readonly AwsService Service;

        // 20120228/us-east-1/iam/aws4_request
        public readonly override string ToString() => $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request";

        public readonly void WriteTo(TextWriter output)
        {
            output.Write(Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            output.Write('/');
            output.Write(Region.Name);
            output.Write('/');
            output.Write(Service.Name);
            output.Write('/');
            output.Write("aws4_request");
        }
    }
}