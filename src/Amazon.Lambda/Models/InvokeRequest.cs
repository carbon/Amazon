using Carbon.Json;

namespace Amazon.Lambda
{
    public class InvokeRequest
    {
        public InvokeRequest(string functionName)
        {
            FunctionName = functionName;
        }

        public InvokeRequest(string functionName, JsonNode payload)
        {
            FunctionName = functionName;
            Payload = payload.ToString(pretty: false);
        }

        public string FunctionName { get; }

        public InvocationType? InvocationType { get; set; }

        public LogType? LogType { get; set; }

        // JSON that you want to provide to your Lambda function as input.
        public string Payload { get; }
    }

    public enum InvocationType
    {
        Event,
        RequestResponse,
        DryRun
    }

    public enum LogType
    {
        None,
        Tail
    }
}
