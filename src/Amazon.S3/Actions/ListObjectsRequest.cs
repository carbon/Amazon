using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Amazon.S3
{
    using Helpers;

    public sealed class ListBucketRequest : S3Request
    {
        public ListBucketRequest(string host, string bucketName, ListBucketOptions options)
            : base(HttpMethod.Get, host, bucketName, null)
        {
            options.QueryList.Add("list-type", "2");

            if (options.QueryList.Count > 0)
            {
                RequestUri = new Uri(RequestUri.ToString() + options.QueryList.ToQueryString());
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
        public Dictionary<string, string> QueryList { get; } = new Dictionary<string, string>();

        public string Delimiter
        {
            get => Get("delimiter"); 
            set => Set("delimiter", value);
        }

        public string Prefix
        {
            get => Get("prefix");
            set => Set("prefix", value);
        }

        public string ContinuationToken
        {
            get => Get("continuation-token");
            set => Set("continuation-token", value);
        }

        public string StartAfter
        {
            get => Get("start-after");
            set => Set("start-after", value);
        }

        public int MaxKeys
        {
            get => int.Parse(QueryList["max-keys"]);
            set => QueryList["max-keys"] = value.ToString();
        }

        private string Get(string name)
        {
            QueryList.TryGetValue(name, out string value);
            
            return value;
        }

        private void Set(string name, string value)
        {
            if (value is null)
            {
                QueryList.Remove(name);
            }
            else
            {
                QueryList[name] = value;
            }
        }
    }
}