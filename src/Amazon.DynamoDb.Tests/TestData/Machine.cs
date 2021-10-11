using System.Net;

using Carbon.Data.Annotations;

namespace Amazon.DynamoDb.Models.Tests;

[Dataset("Machine")]
public class Machine
{
    [Member("id"), Key]
    public int Id { get; set; }

    [Member("ips")]
    public List<IPAddress> Ips { get; set; }
}
