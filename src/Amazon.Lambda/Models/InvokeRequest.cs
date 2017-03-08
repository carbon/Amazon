using System;
using Carbon.Json;

namespace Amazon.Lambda
{
    public class InvokeRequest
    {
        public InvokeRequest(string functionName)
        {
            FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
        }

        public InvokeRequest(string functionName, JsonNode payload)
        {
            FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
            Payload = payload?.ToString(pretty: false) ?? throw new ArgumentNullException(nameof(payload));
        }

        public string FunctionName { get; }

        public InvocationType? InvocationType { get; set; }

        public LogType? LogType { get; set; }

        // JSON that you want to provide to your Lambda function as input.
        public string Payload { get; }
    }

    public enum InvocationType
    {
        Event = 1,
        RequestResponse = 2,
        DryRun =3
    }

    public enum LogType
    {
        None = 0,
        Tail = 1
    }
}