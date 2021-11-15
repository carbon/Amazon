using System.Text.Json.Serialization;

namespace Amazon.Sts.Tests;

[JsonSerializable(typeof(CallerIdentityVerificationParameters))]
public partial class StsJsonContext : JsonSerializerContext
{
}