namespace Amazon.Bedrock.Models;

using System.Text.Json.Serialization;

public class PromptRouterTrace
{
    [JsonPropertyName("invokedModelId")]
    public string InvokedModelId { get; set; }
}
