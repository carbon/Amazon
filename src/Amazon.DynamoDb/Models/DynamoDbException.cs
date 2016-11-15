using System;
using System.Collections.Generic;

using Carbon.Json;

namespace Amazon.DynamoDb
{
    using Scheduling;

    public class DynamoDbException : Exception, IException
    {
        private readonly IList<Exception> exceptions;

        public DynamoDbException(string message)
            : base(message) { }

        public DynamoDbException(string message, Exception innerException)
            : base(message, innerException) { }

        public DynamoDbException(string message, IList<Exception> exceptions)
            : base(message)
        {
            this.exceptions = exceptions;
        }

        public string Type { get; set; }

        public int StatusCode { get; set; }

        public static DynamoDbException Parse(string jsonText)
        {
            return FromJson(JsonObject.Parse(jsonText));
        }

        public IList<Exception> Exceptions => exceptions;

        public static DynamoDbException FromJson(JsonObject json)
        {
            var type = json["__type"].ToString();
            var message = "";

            if (json.ContainsKey("message"))
            {
                message = json["message"].ToString();
            }
            else if (json.ContainsKey("Message"))
            {
                message = json["Message"].ToString();
            }

            if (type != null)
            {
                type = type.Split('#')[1];
            }

            if (type == "ConditionalCheckFailedException")
            {
                return new Conflict(message);
            }

            var error = new DynamoDbException(message) {
                Type = type
            };

            return error;
        }

        public bool IsTransient
        {
            get
            {
                // Client Errors = 4xx (Don't retry)
                // Server Errors = 5xx (Retry)

                switch (Type)
                {
                    case "InternalServerError":
                    case "InternalFailure":
                    case "ProvisionedThroughputExceededException":
                    case "ThrottlingException": return true;
                }

                return false;
            }
        }
    }

    public enum DynamoDbErrorType
    {
        ExpiredTokenException,

        AccessDeniedException,
        ConditionalCheckFailedException,
        IncompleteSignatureException,
        ItemCollectionSizeLimitExceededException,
        LimitExceededException,
        MissingAuthenticationTokenException,
        ProvisionedThroughputExceededException,
        ResourceInUseException,
        ResourceNotFoundException,
        ThrottlingException,
        ValidationException,
        InternalFailure,
        InternalServerError,
        SerializationException,
        ServiceUnavailableException
    }
}

/*
400	AccessDeniedException					Access denied.	General authentication failure. The client did not correctly sign the request. Consult the signing documentation.	N
400	ConditionalCheckFailedException			The conditional request failed.	Example: The expected value did not match what was stored in the system.	N
400	IncompleteSignatureException			The request signature does not conform to AWS standards.	The signature in the request did not include all of the required components. See Calculating the HMAC-SHA256 Signature for Amazon DynamoDB.	N
400	LimitExceededException					Too many operations for a given subscriber.	Example: The number of concurrent table requests (cumulative number of tables in the CREATING, DELETING or UPDATING state) exceeds the maximum allowed of 20. The total limit of tables (currently in the ACTIVE state) is 250.	N
400	MissingAuthenticationTokenException		Request must contain a valid (registered) AWS Access Key ID.	The request did not include the required x-amz-security-token. See Making HTTP Requests to Amazon DynamoDB.	N
400	ProvisionedThroughputExceededException	You exceeded your maximum allowed provisioned throughput.	Example: Your request rate is too high or the request is too large. The AWS SDKs for Amazon DynamoDB automatically retry requests that receive this exception. So, your request is eventually successful, unless the request is too large or your retry queue is too large to finish. Reduce the frequency of requests, using Error Retries and Exponential Backoff. Or, see Specifying Read and Write Requirements (Provisioned Throughput) for other strategies.	Y
400	ResourceInUseException					The resource which is being attempted to be changed is in use.	Example: You attempted to recreate an existing table, or delete a table currently in the CREATING state.	N
400	ResourceNotFoundException				The resource which is being requested does not exist.	Example: Table which is being requested does not exist, or is too early in the CREATING state.	N
400	ThrottlingException						Rate of requests exceeds the allowed throughput.	This can be returned by the control plane API (CreateTable, DescribeTable, etc) when they are requested too rapidly.	Y
400	ValidationException						One or more required parameter values were missing.	One or more required parameter values were missing.	N
413	Request Entity Too Large.				Maximum item size of 1MB exceeded.	N
500	InternalFailure							The server encountered an internal error trying to fulfill the request.	The server encountered an error while processing your request.	Y
500	InternalServerError						The server encountered an internal error trying to fulfill the request.	The server encountered an error while processing your request.	Y
500	ServiceUnavailableException				The service is currently unavailable or busy.	There was an unexpected error on the server while processing your request.	Y
*/


/*

{
  "__type":"com.amazonaws.dynamodb.v20111205#ProvisionedThroughputExceededException",
  "message":"The level of configured provisioned throughput for the table was exceeded. Consider increasing your provisioning level with the UpdateTable API"
}

// {"__type":"com.amazon.coral.service#ExpiredTokenException","message":"The security token included in the request is expired"}

// {"__type":"com.amazon.coral.service#SerializationException","Message":"Start of list found where not expected"}
*/
