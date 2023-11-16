// Based on .NET Source code

namespace System.Text;

internal static class StringBuilderCache
{
    [ThreadStatic]
    static StringBuilder? cachedInstance;

    public static StringBuilder Acquire()
    {
        var sb = cachedInstance;

        if (sb is null)
        {
            return new StringBuilder(128);
        }

        sb.Length = 0;

        cachedInstance = null;

        return sb;
    }

    public static string ExtractAndRelease(StringBuilder sb)
    {
        var text = sb.ToString();

        cachedInstance = sb;

        return text;
    }
}
