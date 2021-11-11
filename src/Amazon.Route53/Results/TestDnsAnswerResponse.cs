#nullable disable

namespace Amazon.Route53;

public sealed class TestDnsAnswerResponse
{
    public string Nameserver { get; init; }

    public string RecordName { get; init; }

    public string RecordType { get; init; }

    public RecordData RecordData { get; init; }

    public string ResponseCode { get; init; }

    public string Protocol { get; init; }
}