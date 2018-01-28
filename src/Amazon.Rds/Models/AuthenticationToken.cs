using System;

namespace Amazon.Rds
{
    public class AuthenticationToken
    {
        public AuthenticationToken(
            string value, 
            DateTime issued, 
            DateTime expires)
        {
            Value   = value ?? throw new ArgumentNullException(nameof(value));
            Issued  = issued;
            Expires = expires;
        }

        public string Value { get; }

        public DateTime Issued { get; }

        public DateTime Expires { get; }
    }
}