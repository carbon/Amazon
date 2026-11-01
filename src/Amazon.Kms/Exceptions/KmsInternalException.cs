using System.Net;

using Amazon.Scheduling;

namespace Amazon.Kms.Exceptions;

public sealed class KmsInternalException(KmsError error, HttpStatusCode statusCode)
    : KmsException(error, statusCode), IException
{
    public bool IsTransient => true;
}
