namespace Amazon.Bedrock;

public sealed class EmbeddingModelInfo(string id, int dimensionCount) : ModelInfo(id)
{
    public int DimensionCount { get; } = dimensionCount;
}