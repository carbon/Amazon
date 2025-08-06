namespace Amazon.Cryptography.Tests;

public static class TestHelper
{
    public static byte[] GetBytes(ReadOnlySpan<char> text)
    {
        var ms = new MemoryStream();

        foreach (var line in text.EnumerateLines())
        {
            if (line[0] is '+' or '|') continue;

            var a = line;

            if (line.Length > 36)
            {
                a = line[..36];
            }

            ms.Write(Convert.FromHexString(a.ToString().Replace(" ", "")));
        }

        return ms.ToArray();
    }
}