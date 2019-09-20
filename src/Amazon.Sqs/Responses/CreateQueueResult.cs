#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models
{
    public class CreateQueueResponse
    {
        [XmlElement("CreateQueueResult")]
        public CreateQueueResult CreateQueueResult { get; set; }

        public static CreateQueueResponse Parse(string xmlText)
        {
            return SqsSerializer<CreateQueueResponse>.Deserialize(xmlText);
        }
    }

    public class CreateQueueResult
    {
        [XmlElement("QueueUrl")]
        public string QueueUrl { get; set; }
    }
}

/*
<CreateQueueResponse xmlns="http://queue.amazonaws.com/doc/2009-02-01/">
	<CreateQueueResult>
		<QueueUrl>http://queue.amazonaws.com/1234/hello</QueueUrl>
	</CreateQueueResult>
</CreateQueueResponse>
*/
