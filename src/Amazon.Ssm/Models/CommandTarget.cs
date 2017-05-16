using System;

namespace Amazon.Ssm
{
    public class CommandTarget
    {
        public CommandTarget() { }

        public CommandTarget(string key, params string[] values)
        {
            Key    = key    ?? throw new ArgumentNullException(nameof(key));
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public string Key { get; set; }

        public string[] Values { get; set; }
    } 
}