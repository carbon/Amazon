namespace Amazon.Bedrock;

public static class FoundationalModals
{
    public static class Anthropic
    {
        public static readonly LargeLanguageModelInfo ClaudeV2   = new("anthropic.claude-v2");
        public static readonly LargeLanguageModelInfo ClauseV2_1 = new("anthropic.claude-v2:1");
    }

    public static class Amazon
    {
        // Amazon Titan Multimodal Embeddings model                                      
        public static readonly EmbeddingModelInfo     TitanEmbedImageV1  = new("amazon.titan-embed-image-v1", 1024);
        public static readonly LargeLanguageModelInfo TitanTextLiteV1    = new("amazon.titan-text-lite-v1") {  MaxTokenCount = 4000 };
        public static readonly LargeLanguageModelInfo TitanTextExpressV1 = new("amazon.titan-text-express-v1") { MaxTokenCount = 8000 };
        public static readonly EmbeddingModelInfo     TitanEmbedTextV1   = new("amazon.titan-embed-text-v1", 1536);
    }

    public static class Meta
    {
        public static readonly LargeLanguageModelInfo Llama213bChatV1 = new("meta.llama2-13b-chat-v1");
    }
}