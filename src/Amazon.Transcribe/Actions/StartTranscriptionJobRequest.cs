using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class StartTranscriptionJobRequest
{
    [JsonPropertyName("ContentRedaction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ContentRedaction? ContentRedaction { get; set; }

    [JsonPropertyName("IdentifyLanguage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IdentifyLanguage { get; set; }

    /// <summary>
    /// Enables automatic multi-language identification in your transcription job request.
    /// Use this parameter if your media file contains more than one language. If your media contains only one language, use IdentifyLanguage instead.
    /// </summary>
    [JsonPropertyName("IdentifyMultipleLanguages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IdentifyMultipleLanguages { get; set; }

    [JsonPropertyName("JobExecutionSettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JobExecutionSettings? JobExecutionSettings { get; set; }

    /// <summary>
    /// A map of plain text, non-secret key:value pairs, known as encryption context pairs, that provide an added layer of security for your data. 
    /// </summary>
    [JsonPropertyName("KMSEncryptionContext")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? KMSEncryptionContext { get; set; }

    /// <summary>
    /// The language code that represents the language spoken in the input media file.
    /// </summary>
    [JsonPropertyName("LanguageCode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LanguageCode { get; set; }

    [JsonPropertyName("LanguageIdSettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, LanguageIdSetting>? LanguageIdSettings { get; set; }

    [JsonPropertyName("LanguageOptions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? LanguageOptions { get; set; }

    [JsonPropertyName("Media")]
    public required Media Media { get; init; }

    /// <summary>
    /// mp3 | mp4 | wav | flac | ogg | amr | webm | m4a
    /// </summary>
    [JsonPropertyName("MediaFormat")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MediaFormat { get; set; }

    /// <summary>
    /// The sample rate, in hertz, of the audio track in your input media file.
    /// </summary>
    [JsonPropertyName("MediaSampleRateHertz")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MediaSampleRateHertz { get; set; }

    [JsonPropertyName("ModelSettings")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ModelSettings? ModelSettings { get; set; }

    [JsonPropertyName("OutputBucketName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? OutputBucketName { get; set; }

    [JsonPropertyName("OutputEncryptionKMSKeyId")]
    public string? OutputEncryptionKMSKeyId { get; set; }

    [JsonPropertyName("OutputKey")]
    public string? OutputKey { get; set; }

    [JsonPropertyName("Settings")]
    public Settings? Settings { get; set; }

    [JsonPropertyName("Subtitles")]
    public Subtitles? Subtitles { get; set; }

    [JsonPropertyName("Tags")]
    public List<Tag>? Tags { get; set; }

    [JsonPropertyName("ToxicityDetection")]
    public List<ToxicityDetection>? ToxicityDetection { get; set; }

    /// <summary>
    /// A unique name, chosen by you, for your transcription job. The name that you specify is also used as the default name of your transcription output file.
    /// </summary>
    [JsonPropertyName("TranscriptionJobName")]
    [StringLength(200)]
    public required string TranscriptionJobName { get; set; }
}
