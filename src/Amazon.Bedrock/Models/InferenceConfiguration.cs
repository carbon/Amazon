﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

public sealed class InferenceConfiguration
{
    /// <summary>
    /// The maximum number of tokens to allow in the generated response. The default value is the maximum allowed value for the model that you are using.
    /// </summary>
    [JsonPropertyName("maxTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxTokens { get; init; }

    /// <summary>
    /// A list of stop sequences. A stop sequence is a sequence of characters that causes the model to stop generating the response.
    /// </summary>
    [JsonPropertyName("stopSequences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? StopSequences { get; init; }

    /// <summary>
    /// The likelihood of the model selecting higher-probability options while generating a response. 
    /// A lower value makes the model more likely to choose higher-probability options, while a higher value makes the model more likely to choose lower-probability options.
    /// </summary>
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(0, 1)]
    public double? Temperature { get; init; }

    /// <summary>
    /// The percentage of most-likely candidates that the model considers for the next token.
    /// For example, if you choose a value of 0.8 for topP, the model selects from the top 80% of the probability distribution of tokens that could be next in the sequence.
    /// </summary>
    [JsonPropertyName("topP")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Range(0, 1)]
    public double? TopP { get; init; }
}