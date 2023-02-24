using System.Net;

using Amazon.Scheduling;

namespace Amazon.Kms.Exceptions;

public sealed class KmsValidationException : KmsException, IException
{
    public KmsValidationException(KmsError error, HttpStatusCode statusCode)
        : base(error, statusCode) { }

    public bool IsTransient => false;
}