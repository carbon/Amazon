namespace Amazon.Helpers;

internal ref struct StringSplitter
{
    private readonly ReadOnlySpan<char> _text;
    private readonly char _separator;

    private int _position;

    public StringSplitter(ReadOnlySpan<char> text, char separator)
    {
        _text = text;
        _separator = separator;
        _position = 0;
    }

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

    public bool IsEof => _position == _text.Length;
}
