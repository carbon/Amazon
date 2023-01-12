#nullable disable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm;

public sealed class Command
{
    [StringLength(36)]
    public string CommandId { get; set; }

    [MaxLength(100)]
    public string Comment { get; set; }

    public int CompletedCount { get; set; }

    public int DeliveryTimedOutCount { get; set; }

    public int ErrorCount { get; set; }

    public string DocumentName { get; set; }

    public string DocumentVersion { get; set; }

    public Timestamp ExpiresAfter { get; set; }

    public string[] InstanceIds { get; set; }

    // Can be a number or %
    public string MaxConcurrency { get; set; }

    public string MaxErrors { get; set; }

    public NotificationConfig NotificationConfig { get; set; }

    public string OutputS3BucketName { get; set; }

    public string OutputS3BucketKeyPrefix { get; set; }

    public Dictionary<string, string[]> Parameters { get; set; }

    public Timestamp? RequestedDateTime { get; set; }

    public string ServiceRole { get; set; }

    public CommandStatus Status { get; set; }

    public string StatusDetails { get; set; }

    public int TargetCount { get; set; }

    public Target[] Targets { get; set; }
}

/*
{ 
    "CommandId": "string",
    "Comment": "string",
    "CompletedCount": number,
    "DocumentName": "string",
    "ErrorCount": number,
    "ExpiresAfter": number,
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
    "RequestedDateTime": number,
    "ServiceRole": "string",
    "Status": "string",
    "StatusDetails": "string",
    "TargetCount": number,
    "Targets": [ 
        { 
        "Key": "string",
        "Values": [ "string" ]
        }
    ]
}
*/
