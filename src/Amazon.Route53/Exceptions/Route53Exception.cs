using System;

namespace Amazon.Route53.Exceptions
{
    public class Route53Exception : Exception
    {
        public Route53Exception(string message)
            : base(message) { }
    }
}
