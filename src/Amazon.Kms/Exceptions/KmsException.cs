using System;
using System.Net;

namespace Amazon.Kms
{
    public class KmsException : Exception
    {
        public KmsException(string message, string type)
           : base(message)
        {
            Type = type;

        }
        internal KmsException(KmsError error, HttpStatusCode statusCode)
            : base(error.Message ?? "KMS ERROR - " + error.Type + "/" + statusCode.ToString())
        {
            Type = error.Type;
        }

        public string Type { get; }
    }
}
