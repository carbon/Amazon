using System.Net;

using Amazon.Exceptions;

namespace Amazon.Kms.Exceptions;

public class KmsException : AwsException
{
    internal KmsException(KmsError error, HttpStatusCode httpStatusCode)
        : base(error.Message ?? $"KMS exception - {error.Type} / {httpStatusCode}", httpStatusCode)
    {
        Error = error;
    }

    public string Type => Error.Type;

    public KmsError Error { get; }
}
