using System;
using System.ComponentModel.DataAnnotations;

using Carbon.Json;

namespace Amazon.Ssm
{
    public class SendCommandRequest : ISsmRequest
    {
        public SendCommandRequest() { }

        public SendCommandRequest(string documentName, string[] instanceIds)
        {
            DocumentName = documentName ?? throw new ArgumentNullException(nameof(documentName));
            InstanceIds  = instanceIds  ?? throw new ArgumentNullException(nameof(instanceIds));
        }

        public SendCommandRequest(string documentName, CommandTarget[] targets)
        {
            DocumentName = documentName ?? throw new ArgumentNullException(nameof(documentName));
            Targets      = targets      ?? throw new ArgumentNullException(nameof(targets));
        }

        public string Comment { get; set; }

        public string DocumentHash { get; set; }

        [Required]
        public string DocumentName { get; set; }

        public string[] InstanceIds { get; set; }

        public string MaxConcurrency { get; set; }

        public string MaxErrors { get; set; }

        public JsonObject Parameters { get; set; }

        public CommandTarget[] Targets { get; set; }
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