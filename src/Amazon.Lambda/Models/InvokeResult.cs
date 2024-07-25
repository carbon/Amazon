#nullable disable

namespace Amazon.Lambda;

public class InvokeResult(byte[] responseBytes)
{
    public string LogResult { get; set; }

    public byte[] ResponseBytes { get; } = responseBytes;
}

/*
You can set this optional parameter to Tail in the request only if you specify the InvocationType parameter with value RequestResponse. 
In this case, AWS Lambda returns the base64-encoded last 4 KB of log data produced by your Lambda function in the x-amz-log-result header.
*/
