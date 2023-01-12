namespace Amazon.Ssm;

public sealed class SendCommandRequest : ISsmRequest
{
    public SendCommandRequest() { }

    public SendCommandRequest(
        string documentName,
        string[] instanceIds,
        Dictionary<string, string[]>? parameters = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(documentName);
        ArgumentNullException.ThrowIfNull(instanceIds);

        DocumentName = documentName;
        InstanceIds = instanceIds;
        Parameters = parameters;
    }

    public SendCommandRequest(
        string documentName,
        CommandTarget[] targets,
        Dictionary<string, string[]>? parameters = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(documentName);
        ArgumentNullException.ThrowIfNull(targets);

        DocumentName = documentName;
        Targets = targets;
        Parameters = parameters;
    }

    public string? Comment { get; set; }

    public string? DocumentHash { get; set; }

    public required string DocumentName { get; set; }

    public string[]? InstanceIds { get; set; }

    public string? MaxConcurrency { get; set; }

    public string? MaxErrors { get; set; }

    public Dictionary<string, string[]>? Parameters { get; set; }

    public CommandTarget[]? Targets { get; set; }
}

/*
{
   "Comment": "string",
   "DocumentHash": "string",
   "DocumentHashType": "string",
   "DocumentName": "string",
   "InstanceIds": [ "string" ],
   "MaxConcurrency": "string",
   "MaxErrors": "string",
   "NotificationConfig": { 
      "NotificationArn": "string",
      "NotificationEvents": [ "string" ],
      "NotificationType": "string"
   },
   "OutputS3BucketName": "string",
   "OutputS3KeyPrefix": "string",
   "OutputS3Region": "string",
   "Parameters": { 
      "string" : [ "string" ]
   },
   "ServiceRoleArn": "string",
   "Targets": [ 
      { 
         "Key": "string",
         "Values": [ "string" ]
      }
   ],
   "TimeoutSeconds": number
}
*/