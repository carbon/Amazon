
using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("Annotations")]
public class Annotation
{
    // public AnnotationType Type { get; set; }

    [Member(1), Key]
    public long Id { get; set; }

    [Member(2)]
    public string Description { get; set; }

    [Member(3)]
    public Position Position { get; set; }

    [Member(4)]
    public float Score { get; set; }

    [Member(5)]
    public float Confidence { get; set; }

    [Member(6)]
    public float Topicality { get; set; }

}
