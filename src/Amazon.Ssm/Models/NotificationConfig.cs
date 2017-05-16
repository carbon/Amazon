namespace Amazon.Ssm
{
    public class NotificationConfig
    {
        public string NotificationArn { get; set; }

        public string NotificationEvents { get; set; }

        public string NotificationType { get; set; }
    }
}


/*
{
   "Command": { 
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
}

*/
