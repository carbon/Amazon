using System.Text.Json.Serialization;

namespace Amazon.CodeBuild;

[JsonConverter(typeof(JsonStringEnumConverter<Language>))]
public enum Language
{
    JAVA,
    PYTHON,
    NODE_JS,
    RUBY,
    GOLANG,
    DOCKER,
    ANDROID,
    DOTNET,
    BASE,
    PHP
}