using System;

namespace Amazon.Kms
{
    public class KmsException : Exception
    {
        public KmsException(string message, string type)
           : base(message)
        {
            Type = type;

        }
        public KmsException(KmsError error)
            : base(error.Message ?? "KMS ERROR")
        {
            Type = error.Type;
        }

        public string Type { get; set; }
    }
}
