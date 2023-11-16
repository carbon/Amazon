using System.Text.Json.Serialization;

namespace Amazon.Ssm;

[JsonConverter(typeof(JsonStringEnumConverter<DocumentType>))]
public enum DocumentType
{
    Command,
    Policy,
    Automation,
    Session,
    Package,
    ApplicationConfiguration,
    ApplicationConfigurationSchema,
    DeploymentStrategy,
    ChangeCalendar,
    // Automation.ChangeTemplate,
    ProblemAnalysis,
    ProblemAnalysisTemplate,
    CloudFormation,
    ConformancePackTemplate
}