using System.Text.Json;

namespace Amazon.Kms.Tests;

public static class JSO
{
    public static readonly JsonSerializerOptions Indented = new() {
        WriteIndented = true
    };
}