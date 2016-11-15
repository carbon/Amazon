using System;

namespace Amazon.Ses
{
    public class SesException : Exception
    {
        private readonly SesError error;

        public SesException(string message)
            : base(message) { }

        public SesException(SesError error)
            : this(error.Message)
        {
            this.error = error;
        }

        public string Type => error.Type;

        public string Code => error.Code;

        public bool IsTransient
        {
            get { return Code == "Throttling" || Code == "ServiceUnavailable"; }
        }
    }
}

/*
MessageRejected
Indicates that the action failed, and the message could not be sent. Check the error stack for a description of what caused the error. For more information about problems that can cause this error, see Amazon SES Email Sending Errors.
400
SendEmail, SendRawEmail
IncompleteSignature
The request signature does not conform to AWS standards.
400
All
InternalFailure
The request processing has failed because of an unknown error, exception, or failure.
500
All
InvalidAction
The requested action or operation is invalid. Verify that the action is typed correctly.
400
All
InvalidClientTokenId
The X.509 certificate or AWS access key ID provided does not exist in our records.
403
All
InvalidParameterCombination
Parameters that must not be used together were used together.
400
All
InvalidParameterValue
An invalid or out-of-range value was supplied for the input parameter.
400
All
InvalidQueryParameter
The AWS query string is malformed, does not adhere to AWS standards.
400
All
MalformedQueryString
The query string contains a syntax error.
404
All
MissingAction
The request is missing an action or a required parameter.
400
All
MissingAuthenticationToken
The request must contain either a valid (registered) AWS access key ID or X.509 certificate.
403
All
MissingParameter
A required parameter for the specified action is not supplied.
400
All
OptInRequired
The AWS access key ID needs a subscription for the service.
403
All
RequestExpired
The request reached the service more than 15 minutes after the date stamp on the request or more than 15 minutes after the request expiration date (such as for pre-signed URLs), or the date stamp on the request is more than 15 minutes in the future.
400
All
ServiceUnavailable
The request failed due to a temporary failure of the server.
503
All
Throttling
The request was denied due to request throttling.
400
All
*/
