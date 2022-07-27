#pragma warning disable IDE0057 // Use range operator

namespace Amazon.Helpers;

public ref struct Splitter
{
    private readonly ReadOnlySpan<char> text;
    private readonly char seperator;

    private int position;

    public Splitter(ReadOnlySpan<char> text, char seperator)
    {
        this.text = text;
        this.seperator = seperator;
        this.position = 0;
    }

    public bool TryGetNext(out ReadOnlySpan<char> result)
    {
        if (IsEof)
        {
            result = default;

            return false;
        }
        int start = position;

        int commaIndex = text.Slice(position).IndexOf(seperator);

        if (commaIndex > -1)
        {
            position += commaIndex + 1;

            result = text.Slice(start, commaIndex);
        }
        else
        {
            position = text.Length;

            result = text.Slice(start);
        }

        return true;
    }

    public bool IsEof => position == text.Length;
}
