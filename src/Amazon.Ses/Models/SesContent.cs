namespace Amazon.Ses;

public sealed class SesContent
{
    public SesContent(string data, CharsetType charset = CharsetType.SevenBitASCII)
    {
        ArgumentNullException.ThrowIfNull(data);

        Charset = charset is CharsetType.UTF8 ? "UTF-8" : null;
        Data = data;
    }

    public string? Charset { get; }

    public string Data { get; }
}

public enum CharsetType
{
    SevenBitASCII = 1,
    UTF8          = 2
}