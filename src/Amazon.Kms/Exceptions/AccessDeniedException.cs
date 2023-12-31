using System.Net;

using Amazon.Scheduling;

namespace Amazon.Kms.Exceptions;

public sealed class AccessDeniedException(KmsError error, HttpStatusCode status)
    : KmsException(error, status), IException
{
    public bool IsTransient => false;
}