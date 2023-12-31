using System.ComponentModel.DataAnnotations;

namespace Amazon.Sts;

public sealed class GetSessionTokenRequest : IStsRequest
{
    string IStsRequest.Action => "GetSessionToken";

    [Range(900, 129600)]
    public int? DurationInSeconds { get; init; }

    [StringLength(256)]
    public string? SerialNumber { get; init; }

    [StringLength(6)]
    public string? TokenCode { get; init; }
}