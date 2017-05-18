using System;
using Carbon.Json;

namespace Amazon.Ssm
{
    public class Command
    {
        public string CommandId { get; set; }

        public string Comment { get; set; }

        public int CompletedCount { get; set; }

        public int ErrorCount { get; set; }

        public string DocumentName { get; set; }

        // scientific notation: 1.494825472676E9 
        public DateTime ExpiresAfter { get; set; }

        public string[] InstanceIds { get; set; }

        // Can be a number or %
        public string MaxConcurrency { get; set; }

        public string MaxErrors { get; set; }

        public NotificationConfig NotificationConfig { get; set; }

        public string OutputS3BucketName { get; set; }

        public string OutputS3BucketKeyPrefix { get; set; }

        public JsonObject Parameters { get; set; }

        public DateTime RequestedDateTime { get; set; }

        public string ServiceRole { get; set; }

        public string Status { get; set; }

        public string StatusDetails { get; set; }

        public int TargetCount { get; set; }

        public Target[] Targets { get; set; }
    }
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
