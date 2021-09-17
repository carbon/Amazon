using System.Net;

using Amazon.Scheduling;

namespace Amazon.Kms.Exceptions;

public sealed class AccessDeniedException : KmsException, IException
{
    public AccessDeniedException(KmsError error, HttpStatusCode status)
        : base(error, status) { }

    public bool IsTransient => false;
}