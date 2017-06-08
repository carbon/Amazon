using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3
{
    public class GetObjectRequest : S3Request
    {
        public GetObjectRequest(AwsRegion region, string bucketName, string objectName)
            : base(HttpMethod.Get, region, bucketName, objectName)
        {
            CompletionOption = HttpCompletionOption.ResponseHeadersRead;
        }
        
        public DateTimeOffset? IfModifiedSince
        {
            get => Headers.IfModifiedSince;
            set => Headers.IfModifiedSince = value;
        }

        public string IfNoneMatch
        {
            set
            {
                if (value == null)
                {
                    Headers.IfNoneMatch.Clear();
                }
                else
                {
                    Headers.IfNoneMatch.Add(new EntityTagHeaderValue(value));
                }
            }
        }

        public void SetRange(long? from, long? to)
        {
            Headers.Range = new RangeHeaderValue(from, to);
        }
    }
}