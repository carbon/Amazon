namespace Amazon.Bedrock;

// https://docs.aws.amazon.com/bedrock/latest/userguide/inference-profiles-support.html
// https://docs.aws.amazon.com/bedrock/latest/userguide/model-cards.html

public static class FoundationalModals
{
    public static class Anthropic
    {
        public static readonly FoundationModel ClaudeSonnet_4_5 = new("claude-sonnet-4-5-20250929-v1:0", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.anthropic.claude-sonnet-4-5-20250929-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000
        };

        // https://docs.aws.amazon.com/bedrock/latest/userguide/model-card-anthropic-claude-sonnet-4-6.html
        // Knowledge cutoff: Aug 2025
        public static readonly FoundationModel ClaudeSonnet_4_6 = new("anthropic.claude-sonnet-4-6", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.AU | InferenceProfileRegionFlags.EU | InferenceProfileRegionFlags.JP | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.anthropic.claude-sonnet-4-6",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 1_000_000,
            MaxOutputTokenCount = 64_000
        };

        public static readonly FoundationModel ClaudeSonnet_5 = new("anthropic.claude-sonnet-5", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.AU | InferenceProfileRegionFlags.EU | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.anthropic.claude-sonnet-5",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 1_000_000,
            MaxOutputTokenCount = 64_000
        };

        public static readonly FoundationModel ClaudeSonnet_5_5 = new("anthropic.claude-sonnet-5-5", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.AU | InferenceProfileRegionFlags.EU | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.anthropic.claude-sonnet-5-5",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 1_000_000,
            MaxOutputTokenCount = 64_000
        };

        public static readonly FoundationModel ClaudeHaiku_4_5 = new("anthropic.claude-haiku-4-5-20251001-v1:0", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.EU | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.anthropic.claude-haiku-4-5-20251001-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000
        };

        public static readonly FoundationModel ClaudeHaiku_5_5 = new("anthropic.anthropic.claude-haiku-5-5", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.AU | InferenceProfileRegionFlags.EU | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.anthropic.claude-haiku-5-5",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 1_000_000,
            MaxOutputTokenCount = 64_000
        };
    }


    public static class DeepSeek
    {
        public static readonly FoundationModel DeepSeek_3_2 = new("deepseek.v3.2") {
            Regions = [AwsRegion.USEast1, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000
        };
    }

    public static class XAI
    {
        public static readonly FoundationModel Grok_4_6 = new("xai.grok-4.6", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.xai.grok-4.6",
            Regions = [AwsRegion.USEast1, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000
        };
    }

    public static class OpenAI
    {
        public static readonly FoundationModel Gpt6Luna = new("openai.gpt-6-luna", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.openai.gpt-6-luna",
            Regions = [AwsRegion.USEast1, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000,
            ZeroRetention = false
        };

        public static readonly FoundationModel Gpt6_1_Sol = new("openai.gpt-6.1-sol", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.openai.gpt-6-1-sol",
            Regions = [AwsRegion.USEast1, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000,
            ZeroRetention = false
        };


        public static readonly FoundationModel Gpt56Luna = new("openai.gpt-5.6-luna", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.openai.gpt-5.6-luna",
            Regions = [AwsRegion.USEast1, AwsRegion.USWest1, AwsRegion.USWest2],
            MaxInputTokenCount = 200_000,
            ZeroRetention = false
        };
    }

    public static class Amazon
    {
        public static readonly FoundationModel NovaMicro = new("amazon.nova-micro-v1:0", InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.amazon.nova-micro-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        };

        public static readonly FoundationModel NovaLite = new("amazon.nova-lite-v1:0", InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.amazon.nova-lite-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // Image, Video, Text

        public static readonly FoundationModel NovaPro = new("amazon.nova-pro-v1:0", InferenceProfileRegionFlags.US) {
            InferenceProfile = "us.amazon.nova-pro-v1:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // Image, Video, Text

        public static readonly FoundationModel Nova2MultimodalEmbeddings = new("amazon.nova-2-multimodal-embeddings-v1:0", InferenceProfileRegionFlags.Global | InferenceProfileRegionFlags.US) {
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest2]
        }; // Image, Video, Text

        public static readonly RerankModelInfo    RerankV1           = new("amazon.rerank-v1:0");
        public static readonly EmbeddingModelInfo TitanEmbedImageV1  = new("amazon.titan-embed-image-v1", [256, 384, 1024]);
    }

    public static class Cohere
    {
        public static readonly RerankModelInfo RerankV3_5 = new("cohere.rerank-v3-5:0");

        public static readonly EmbeddingModelInfo EmbedV4 = new("cohere.embed-v4:0", [256, 512, 1024, 1536]) {
            InferenceProfile = "us.cohere.embed-v4:0",
            Regions = [AwsRegion.USEast1, AwsRegion.USEast2, AwsRegion.USWest1, AwsRegion.USWest2],
        };
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

public sealed class FoundationModel(string id, InferenceProfileRegionFlags inferenceProfileRegionFlags = default) 
    : ModelInfo(id, inferenceProfileRegionFlags) {

    public bool UseMantle { get; init; } = false;

    public bool ZeroRetention { get; init; } = true;
}

public sealed class EmbeddingModelInfo(string id, int[] dimensionCount) : ModelInfo(id)
{
    public int[] SupportedDimensions { get; } = dimensionCount;
}


public sealed class RerankModelInfo(string id) : ModelInfo(id)
{
}


// https://docs.aws.amazon.com/bedrock/latest/userguide/model-cards.html