using System.Net;

using Amazon.Ec2.Models;
using Amazon.Exceptions;

namespace Amazon.Ec2.Exceptions;

public sealed class Ec2Exception : AwsException
{
    public Ec2Exception(string message, HttpStatusCode httpStatusCode)
        : base(message, httpStatusCode) { }

    public Ec2Exception(Error[] errors, HttpStatusCode httpStatusCode)
        : base(GetMessage(errors), httpStatusCode)
    {
        Errors = errors;
    }

    public Error[]? Errors { get; }

    private static string GetMessage(Error[] errors)
    {
        if (errors.Length is 1)
        {
            return errors[0].Message;
        }

        return "Multiple errors occurred processing the request. See inner Errors[] for details.";
    }
}