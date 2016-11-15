using System;
using System.Xml.Linq;

namespace Amazon.Sqs.Models
{
    public class CreateQueueResult
    {
        public Uri QueueUrl { get; set; }

        public static CreateQueueResult Parse(string xmlText)
        {
            var rootEl = XElement.Parse(xmlText); // CreateQueueResponse

            var queueUrl = rootEl.Element(SqsClient.NS + "CreateQueueResult").Element(SqsClient.NS + "QueueUrl").Value;

            return new CreateQueueResult {
                QueueUrl = new Uri(queueUrl)
            };
        }
    }
}

/*
<CreateQueueResponse xmlns="http://queue.amazonaws.com/doc/2009-02-01/">
	<CreateQueueResult>
		<QueueUrl>http://queue.amazonaws.com/416372880389/hello</QueueUrl>
	</CreateQueueResult>
</CreateQueueResponse>
*/
