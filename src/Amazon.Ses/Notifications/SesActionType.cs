using System.Text.Json.Serialization;

namespace Amazon.Ses;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SesActionType
{
    S3       = 1,
    SNS      = 2,
    Bounce   = 3,
    Lambda   = 4,
    Stop     = 5,
    WorkMail = 6
}