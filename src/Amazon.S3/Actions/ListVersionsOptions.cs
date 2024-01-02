using System.Globalization;

namespace Amazon.S3;

public sealed class ListVersionsOptions
{
    private readonly Dictionary<string, string> _items = [];

    public string? Delimiter
    {
        get => Get("delimiter");
        init => Set("delimiter", value);
    }

    public string? Prefix
    {
        get => Get("prefix");
        init => Set("prefix", value);
    }

    public string? VersionIdMarker
    {
        get => Get("version-id-marker");
        init => Set("version-id-marker", value);
    }

    public string? KeyMarker
    {
        get => Get("key-marker");
        init => Set("key-marker", value);
    }

    public string? EncodingType
    {
        get => Get("encoding-type");
        init => Set("encoding-type", value);
    }

    public int? MaxKeys
    {
        get
        {
            if (Get("max-keys") is string maxKeys)
            {
                return int.Parse(maxKeys, CultureInfo.InvariantCulture);
            }

            return null;
        }
        init
        {
            Set("max-keys", value is int maxKeys ? maxKeys.ToString(CultureInfo.InvariantCulture) : null);
        }
    }

    private string? Get(string name)
    {
        _items.TryGetValue(name, out string? value);

        return value;
    }

    private void Set(string name, string? value)
    {
        if (value is null)
        {
            _items.Remove(name);
        }
        else
        {
            _items[name] = value;
        }
    }

    internal Dictionary<string, string> Items => _items;
}
