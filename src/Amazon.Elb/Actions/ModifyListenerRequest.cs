#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class ModifyListenerRequest : IElbRequest
{
    public string Action => "ModifyListener";

    public Certificate[] Certificates { get; init; }

    public Action[] DefaultActions { get; init; }

    [Required]
    public string ListenerArn { get; init; }

    public int? Port { get; init; }

    public string Protocol { get; init; }

    public string SslPolicy { get; init; }
}