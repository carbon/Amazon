using System;

namespace Amazon
{
    public class CredentialScope
    {
        public CredentialScope(DateTime date, AwsRegion region, AwsService service)
        {
            #region Preconditions

            if (region == null)
                throw new ArgumentNullException(nameof(region));

            if (service == null)
                throw new ArgumentNullException(nameof(service));

            #endregion

            Date = date;
            Region = region;
            Service = service;
        }

        public DateTime Date { get; }

        public AwsRegion Region { get; }

        public AwsService Service { get; }

        // 20120228/us-east-1/iam/aws4_request
        public override string ToString() => 
            $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request";
    }
}