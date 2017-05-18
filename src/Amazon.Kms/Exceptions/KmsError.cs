using System;
using System.Runtime.Serialization;

namespace Amazon.Kms
{
    public class KmsError
    {
        [DataMember(Name = "__type")]
        public string Type { get; set; }
        
        public string Message { get; set; }
    }

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

    public class AccessDeniedException : KmsException
    {
        public AccessDeniedException(string message)
            : base("AccessDeniedException", message) { }
    }
}


// { "__type":"UnknownOperationException"}
// {"__type":"NotFoundException","message":"Key 'arn:aws:kms:us-east-1:416372880389:key/master' does not exist"}
