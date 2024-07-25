using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public class TranscriptionJob
{
    [JsonPropertyName("CompletionTime")]
    public double CompletionTime { get; set; }

    [JsonPropertyName("ContentRedaction")]
    public ContentRedaction? ContentRedaction { get; set; }

    [JsonPropertyName("CreationTime")]
    public double CreationTime { get; set; }

    [JsonPropertyName("FailureReason")]
    public string? FailureReason { get; set; }

    [JsonPropertyName("IdentifiedLanguageScore")]
    public double IdentifiedLanguageScore { get; set; }

    [JsonPropertyName("IdentifyLanguage")]
    public bool IdentifyLanguage { get; set; }

    [JsonPropertyName("IdentifyMultipleLanguages")]
    public bool IdentifyMultipleLanguages { get; set; }

    [JsonPropertyName("JobExecutionSettings")]
    public JobExecutionSettings? JobExecutionSettings { get; set; }

    [JsonPropertyName("LanguageCode")]
    public string? LanguageCode { get; set; }

    [JsonPropertyName("LanguageCodes")]
    public List<LanguageCodeItem> LanguageCodes { get; set; }

    [JsonPropertyName("LanguageIdSettings")]
    public Dictionary<string, LanguageIdSetting> LanguageIdSettings { get; set; }

    [JsonPropertyName("LanguageOptions")]
    public List<string> LanguageOptions { get; set; }

    [JsonPropertyName("Media")]
    public Media Media { get; set; }

    [JsonPropertyName("MediaFormat")]
    public string MediaFormat { get; set; }

    [JsonPropertyName("MediaSampleRateHertz")]
    public int MediaSampleRateHertz { get; set; }

    [JsonPropertyName("ModelSettings")]
    public ModelSettings? ModelSettings { get; set; }

    [JsonPropertyName("Settings")]
    public Settings? Settings { get; set; }

    [JsonPropertyName("StartTime")]
    public double StartTime { get; set; }

    [JsonPropertyName("Subtitles")]
    public Subtitles? Subtitles { get; set; }

    [JsonPropertyName("Tags")]
    public List<Tag>? Tags { get; set; }

    [JsonPropertyName("ToxicityDetection")]
    public List<ToxicityDetection>? ToxicityDetection { get; set; }

    [JsonPropertyName("Transcript")]
    public Transcript? Transcript { get; set; }

    [JsonPropertyName("TranscriptionJobName")]
    public string? TranscriptionJobName { get; set; }

    [JsonPropertyName("TranscriptionJobStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TranscriptionJobStatus TranscriptionJobStatus { get; set; }
}