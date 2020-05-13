using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class SendCommandRequest : ISsmRequest
    {
#nullable disable
        public SendCommandRequest() { }
#nullable enable

        public SendCommandRequest(string documentName, string[] instanceIds, Dictionary<string, string[]>? parameters = null)
        {
            DocumentName = documentName ?? throw new ArgumentNullException(nameof(documentName));
            InstanceIds  = instanceIds  ?? throw new ArgumentNullException(nameof(instanceIds));
            Parameters   = parameters;
        }

        public SendCommandRequest(string documentName, CommandTarget[] targets, Dictionary<string, string[]>? parameters = null)
        {
            DocumentName = documentName ?? throw new ArgumentNullException(nameof(documentName));
            Targets      = targets      ?? throw new ArgumentNullException(nameof(targets));
            Parameters   = parameters;
        }

        public string? Comment { get; set; }

        public string? DocumentHash { get; set; }

        [Required]
        public string DocumentName { get; set; }

        public string[]? InstanceIds { get; set; }

#nullable enable

        public string? MaxConcurrency { get; set; }

        public string? MaxErrors { get; set; }

        public Dictionary<string, string[]>? Parameters { get; set; }

#nullable disable

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