using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public class StartTranscriptionJobRequest
{
    [JsonPropertyName("ContentRedaction")]
    public ContentRedaction ContentRedaction { get; set; }

    [JsonPropertyName("IdentifyLanguage")]
    public bool IdentifyLanguage { get; set; }

    [JsonPropertyName("IdentifyMultipleLanguages")]
    public bool IdentifyMultipleLanguages { get; set; }

    [JsonPropertyName("JobExecutionSettings")]
    public JobExecutionSettings JobExecutionSettings { get; set; }

    [JsonPropertyName("KMSEncryptionContext")]
    public Dictionary<string, string> KMSEncryptionContext { get; set; }

    [JsonPropertyName("LanguageCode")]
    public string LanguageCode { get; set; }

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
    public ModelSettings ModelSettings { get; set; }

    [JsonPropertyName("OutputBucketName")]
    public string OutputBucketName { get; set; }

    [JsonPropertyName("OutputEncryptionKMSKeyId")]
    public string OutputEncryptionKMSKeyId { get; set; }

    [JsonPropertyName("OutputKey")]
    public string OutputKey { get; set; }

    [JsonPropertyName("Settings")]
    public Settings Settings { get; set; }

    [JsonPropertyName("Subtitles")]
    public Subtitles Subtitles { get; set; }

    [JsonPropertyName("Tags")]
    public List<Tag> Tags { get; set; }

    [JsonPropertyName("ToxicityDetection")]
    public List<ToxicityDetection> ToxicityDetection { get; set; }

    [JsonPropertyName("TranscriptionJobName")]
    public string TranscriptionJobName { get; set; }
}