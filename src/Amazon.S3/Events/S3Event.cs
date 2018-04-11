using System;
using System.Runtime.Serialization;

namespace Amazon.S3.Events
{
    public sealed class S3Event
    {
        [DataMember(Name = "eventVersion")]
        public string EventVersion { get; set; } // 2.0 d

        [DataMember(Name = "eventSource")]
        public string EventSource { get; set; } // aws:s3

        [DataMember(Name = "awsRegion")]
        public string AwsRegion { get; set; } // us-east-1

        [DataMember(Name = "eventTime")]
        public DateTime EventTime { get; set; } // 2017-10-31T23:52:06.033Z

        [DataMember(Name = "eventName")]
        public string EventName { get; set; } // ObjectCreated:Put

        [DataMember(Name = "userIdentity")]
        public S3UserIdentity UserIdentity { get; set; }

        [DataMember(Name = "s3")]
        public S3EventDetails S3 { get; set; }     
    }
}


// https://docs.aws.amazon.com/AmazonS3/latest/dev/notification-content-structure.html