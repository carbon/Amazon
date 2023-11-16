namespace Amazon.Helpers;

internal ref struct StringSplitter(ReadOnlySpan<char> text, char separator)
{
    private readonly ReadOnlySpan<char> _text = text;
    private readonly char _separator = separator;
    private int _position = 0;

    public bool TryGetNext(out ReadOnlySpan<char> result)
    {
        if (IsEof)
        {
            result = default;

            return false;
        }

        int start = _position;

        int commaIndex = _text.Slice(_position).IndexOf(_separator);

        if (commaIndex > -1)
        {
            _position += commaIndex + 1;

            result = _text.Slice(start, commaIndex);
        }
        else
        {
            _position = _text.Length;

            result = _text.Slice(start);
        }

        return true;
    }

    public readonly bool IsEof => _position == _text.Length;
}
