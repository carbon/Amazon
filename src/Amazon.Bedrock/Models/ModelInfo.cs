namespace Amazon.Bedrock;

public abstract class ModelInfo(string id)
{
    public string Id { get; } = id;

    public int? MaxInputTokenCount { get; init; }

    public string? InferenceProfile { get; init; }

    public AwsRegion[]? Regions { get; init; }

    public static implicit operator string(ModelInfo model)
    {
        return model.InferenceProfile ?? model.Id;
    }
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/model-ids-arns.html