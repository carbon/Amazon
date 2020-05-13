using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class Target
    {
#nullable disable
        public Target() { }
#nullable enable

        public Target(string key, string[] values)
        {
            Key    = key    ?? throw new ArgumentNullException(nameof(key));
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        [StringLength(128)]
        public string Key { get; set; }

        public string[] Values { get; set; }
    }
}