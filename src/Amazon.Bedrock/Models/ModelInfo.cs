namespace Amazon.Bedrock;

public abstract class ModelInfo(string id, InferenceProfileRegionFlags inferenceProfileRegionFlags = default)
{
    public string Id { get; } = id;

    public int? MaxInputTokenCount { get; init; }

    public string? InferenceProfile { get; init; }

    public InferenceProfileRegionFlags InferenceProfileRegionFlags { get; } = inferenceProfileRegionFlags;

    public AwsRegion[]? Regions { get; init; }

    public static implicit operator string(ModelInfo model)
    {
        return model.InferenceProfile ?? model.Id;
    }
}

[Flags]
public enum InferenceProfileRegionFlags
{
    Global = 1 << 1, // global (Global cross-region inference profile)
    EU     = 1 << 2, // eu
    US     = 1 << 3, // us
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/inference-profiles-support.html
// https://docs.aws.amazon.com/bedrock/latest/userguide/model-ids-arns.html