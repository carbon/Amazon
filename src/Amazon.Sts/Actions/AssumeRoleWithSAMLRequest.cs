namespace Amazon.Sts;

public sealed class AssumeRoleWithSamlRequest : IStsRequest
{
    string IStsRequest.Action => "AssumeRoleWithSAML";
}