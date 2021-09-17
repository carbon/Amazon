using System.Net;

using Amazon.Scheduling;

namespace Amazon.Kms.Exceptions;

public sealed class KeyUnavailableException : KmsException, IException
{
    public KeyUnavailableException(KmsError error, HttpStatusCode statusCode)
        : base(error, statusCode) { }

    public bool IsTransient => true;
}