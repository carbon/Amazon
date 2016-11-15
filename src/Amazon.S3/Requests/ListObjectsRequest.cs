using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Amazon.S3
{
    using Extensions;

    public class ListBucketRequest : S3Request
    {
        public ListBucketRequest(AwsRegion region, string bucketName, ListBucketOptions options)
            : base(HttpMethod.Get, region, bucketName, null)
        {
            if (options.QueryList.Count > 0)
            {
                RequestUri = new Uri(this.RequestUri.ToString() + options.QueryList.ToQueryString());
            }

            CompletionOption = HttpCompletionOption.ResponseContentRead;

            /*
			GET ?prefix=photos/2006/&delimiter=/ HTTP/1.1
			Host: quotes.s3.amazonaws.com
			Date: Wed, 01 Mar  2009 12:00:00 GMT
			Authorization: AWS 15B4D3461F177624206A:xQE0diMbLRepdf3YB+FIEXAMPLE=
			*/
        }
    }

    public class ListBucketOptions
    {
        public string Delimiter
        {
            get { return QueryList["delimiter"]; }
            set { QueryList["delimiter"] = value; }
        }

        public string Prefix
        {
            get { return QueryList["prefix"]; }
            set { QueryList["prefix"] = value; }
        }

        public string KeyMarker
        {
            get { return QueryList["key-marker"]; }
            set { QueryList["key-marker"] = value; }
        }

        public int MaxKeys
        {
            get { return int.Parse(QueryList["max-keys"]); }
            set { QueryList["max-keys"] = value.ToString(); }
        }

        public Dictionary<string, string> QueryList { get; } = new Dictionary<string, string>();
    }
}