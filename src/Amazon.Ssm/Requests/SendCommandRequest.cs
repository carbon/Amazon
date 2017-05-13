using Carbon.Json;

namespace Amazon.Ssm
{
    public class SendCommandRequest
    {
        public string Comment { get; set; }

        public string DocumentHash { get; set; }

        public string DocumentName { get; set; }

        public string[] InstanceIds { get; set; }

        public string MaxConcurrency { get; set; }

        public string MaxErrors { get; set; }

        public JsonObject Parameters { get; set; }

        public CommandTarget[] Targets { get; set; }
    }

    public class CommandTarget
    {
        public string Key { get; set; }

        public string[] Values { get; set; }
    }
 
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