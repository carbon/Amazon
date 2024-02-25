using System.Net;

namespace Amazon.Kms.Exceptions;

public sealed class DryRunOperationException(KmsError error) 
    : KmsException(error, HttpStatusCode.BadRequest)
{
}

// 400 | DryRunOperationException