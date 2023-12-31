namespace Amazon.Bedrock;

public abstract class ModelInfo(string id)
{
    public string Id { get; } = id;

    public int? MaxTokenCount { get; init; }

    public static implicit operator string(ModelInfo model)
    {
        return model.Id;
    }
}