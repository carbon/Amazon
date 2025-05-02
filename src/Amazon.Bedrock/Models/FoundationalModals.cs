namespace Amazon.Bedrock;

// https://docs.aws.amazon.com/bedrock/latest/userguide/inference-profiles-support.html

public static class FoundationalModals
{
    public static class Anthropic
    {
        public static readonly FoundationModel ClaudeV3_5_Haiku = new("anthropic.claude-3-5-haiku-20241022-v1:0") {
            InferenceProfile = "us.anthropic.claude-3-5-haiku-20241022-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000
        }; 
        
        public static readonly FoundationModel ClaudeV3_5_SonnetV1 = new("anthropic.claude-3-5-sonnet-20240620-v1:0");
        public static readonly FoundationModel ClaudeV3_5_SonnetV2 = new("anthropic.claude-3-5-sonnet-20241022-v2:0") {
            InferenceProfile = "us.anthropic.claude-3-5-sonnet-20241022-v2:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000 
        };

        public static readonly FoundationModel ClaudeV3_7_Sonnet = new("anthropic.claude-3-7-sonnet-20250219-v1:0") {
            InferenceProfile = "us.anthropic.claude-3-7-sonnet-20250219-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000
        };
    }

    public static class Amazon
    {
        public static readonly FoundationModel NovaMicro = new("amazon.nova-micro-v1:0") {
            InferenceProfile = "us.amazon.nova-micro-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        };

        public static readonly FoundationModel NovaLite = new("amazon.nova-lite-v1:0") {
            InferenceProfile = "us.amazon.nova-lite-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // Image, Video, Text

        public static readonly FoundationModel NovaPro = new("amazon.nova-pro-v1:0") {
            InferenceProfile = "us.amazon.nova-pro-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // Image, Video, Text

        public static readonly RerankModelInfo        RerankV1           = new("amazon.rerank-v1:0");
        public static readonly EmbeddingModelInfo     TitanEmbedImageV1  = new("amazon.titan-embed-image-v1", [256, 384, 1024]);
        public static readonly FoundationModel        TitanTextLiteV1    = new("amazon.titan-text-lite-v1")    { MaxInputTokenCount = 4000 };
        public static readonly FoundationModel        TitanTextExpressV1 = new("amazon.titan-text-express-v1") { MaxInputTokenCount = 8000 };
        public static readonly EmbeddingModelInfo     TitanEmbedTextV1   = new("amazon.titan-embed-text-v1",   [1536]);
        public static readonly EmbeddingModelInfo     TitanEmbedTextV2   = new("amazon.titan-embed-text-v2:0", [256, 512, 1024]) { MaxInputTokenCount = 8192 };
    }

    public static class Cohere
    {
        public static readonly RerankModelInfo    RerankV3_5          = new("cohere.rerank-v3-5:0");
        public static readonly EmbeddingModelInfo EmbedEnglishV3      = new("cohere.embed-english-v3", [1024]);
        public static readonly EmbeddingModelInfo EmbedMultilingualV3 = new("cohere.embed-multilingual-v3", [1024]);
    }

    public static class Meta
    {
        public static readonly FoundationModel Llama3_2_11B_Instruct = new("meta.llama3-2-11b-instruct-v1:0") {
            InferenceProfile = "us.meta.llama3-2-11b-instruct-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // vision

        public static readonly FoundationModel Llama3_2_90B_Instruct = new("meta.llama3-2-90b-instruct-v1:0") {
            InferenceProfile = "us.meta.llama3-2-90b-instruct-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // vision
    }
}

public sealed class FoundationModel(string id) : ModelInfo(id) { }

public sealed class EmbeddingModelInfo(string id, int[] dimensionCount) : ModelInfo(id)
{
    public int[] SupportedDimensions { get; } = dimensionCount;
}


public sealed class RerankModelInfo(string id) : ModelInfo(id)
{
}


// https://docs.aws.amazon.com/bedrock/latest/userguide/model-ids-arns.html