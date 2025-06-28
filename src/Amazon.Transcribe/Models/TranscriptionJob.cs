using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public class TranscriptionJob
{
    /// <summary>
    /// The date and time the specified transcription job finished processing.
    /// </summary>
    [JsonPropertyName("CompletionTime")]
    public DateTimeOffset? CompletionTime { get; init; }

    [JsonPropertyName("ContentRedaction")]
    public ContentRedaction? ContentRedaction { get; init; }

    [JsonPropertyName("CreationTime")]
    public double CreationTime { get; init; }

    [JsonPropertyName("FailureReason")]
    public string? FailureReason { get; init; }

    [JsonPropertyName("IdentifiedLanguageScore")]
    public double? IdentifiedLanguageScore { get; init; }

    [JsonPropertyName("IdentifyLanguage")]
    public bool? IdentifyLanguage { get; init; }

    [JsonPropertyName("IdentifyMultipleLanguages")]
    public bool? IdentifyMultipleLanguages { get; init; }

    [JsonPropertyName("JobExecutionSettings")]
    public JobExecutionSettings? JobExecutionSettings { get; init; }

    [JsonPropertyName("LanguageCode")]
    public string? LanguageCode { get; init; }

    [JsonPropertyName("LanguageCodes")]
    public List<LanguageCodeItem>? LanguageCodes { get; init; }

    [JsonPropertyName("LanguageIdSettings")]
    public Dictionary<string, LanguageIdSetting>? LanguageIdSettings { get; init; }

    [JsonPropertyName("LanguageOptions")]
    public List<string>? LanguageOptions { get; init; }

    [JsonPropertyName("Media")]
    public Media Media { get; init; }

    [JsonPropertyName("MediaFormat")]
    public string MediaFormat { get; init; }

    [JsonPropertyName("MediaSampleRateHertz")]
    public int MediaSampleRateHertz { get; init; }

    [JsonPropertyName("ModelSettings")]
    public ModelSettings? ModelSettings { get; init; }

    [JsonPropertyName("Settings")]
    public Settings? Settings { get; init; }

    [JsonPropertyName("StartTime")]
    public DateTimeOffset? StartTime { get; init; }

    [JsonPropertyName("Subtitles")]
    public Subtitles? Subtitles { get; init; }

    [JsonPropertyName("Tags")]
    public List<Tag>? Tags { get; init; }

    [JsonPropertyName("ToxicityDetection")]
    public List<ToxicityDetection>? ToxicityDetection { get; init; }

    [JsonPropertyName("Transcript")]
    public Transcript? Transcript { get; init; }

    [JsonPropertyName("TranscriptionJobName")]
    public string? TranscriptionJobName { get; init; }

    [JsonPropertyName("TranscriptionJobStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TranscriptionJobStatus TranscriptionJobStatus { get; init; }
}