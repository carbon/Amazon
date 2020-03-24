using System.Text.Json;

namespace Amazon.Kms.Tests
{
    public static class JSO
    {
        public static readonly JsonSerializerOptions Default = new JsonSerializerOptions { WriteIndented = true, IgnoreNullValues = true };
    }
}
