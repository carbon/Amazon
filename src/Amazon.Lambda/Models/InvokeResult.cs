#nullable disable

namespace Amazon.Lambda
{
    public class InvokeResult
    {
        public InvokeResult(string responseText)
        {
            ResponseText = responseText;
        }

        public string LogResult { get; set; }

        public string ResponseText { get; }
    }
}

/*
You can set this optional parameter to Tail in the request only if you specify the InvocationType parameter with value RequestResponse. 
In this case, AWS Lambda returns the base64-encoded last 4 KB of log data produced by your Lambda function in the x-amz-log-result header.
*/
