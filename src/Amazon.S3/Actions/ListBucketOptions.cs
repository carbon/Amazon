using System.Collections.Generic;
using System.Globalization;

namespace Amazon.S3
{
    public sealed class ListBucketOptions
    {
        private readonly Dictionary<string, string> items = new (5) {
            { "list-type", "2" } // https://docs.aws.amazon.com/AmazonS3/latest/API/API_ListObjectsV2.html
        };

        public string? Delimiter
        {
            get => Get("delimiter"); 
            set => Set("delimiter", value);
        }

        public string? Prefix
        {
            get => Get("prefix");
            set => Set("prefix", value);
        }

        public string? ContinuationToken
        {
            get => Get("continuation-token");
            set => Set("continuation-token", value);
        }

        public string? StartAfter
        {
            get => Get("start-after");
            set => Set("start-after", value);
        }

        /*
        public string? EncodingType
        {
            get => Get("encoding-type");
            set => Set("encoding-type", value);
        }
        */

        public int? MaxKeys
        {
            get
            {
                if (Get("max-keys") is string maxKeys)
                {
                    return int.Parse(maxKeys, CultureInfo.InvariantCulture);
                }

                return null;
            }
            set
            {
                Set("max-keys", value is int maxKeys ? maxKeys.ToString(CultureInfo.InvariantCulture) : null);
            }
        }

        private string? Get(string name)
        {
            items.TryGetValue(name, out string? value);
            
            return value;
        }

        private void Set(string name, string? value)
        {
            if (value is null)
            {
                items.Remove(name);
            }
            else
            {
                items[name] = value;
            }
        }

        internal Dictionary<string, string> Items => items;
    }
}