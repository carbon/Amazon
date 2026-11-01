using System.Globalization;

namespace Amazon.CloudWatch.Serialization;

internal static class CloudWatchParameters
{
    public static void AddOptional(this List<KeyValuePair<string, string>> parameters, string name, string? value)
    {
        if (value != null)
        {
            parameters.Add(new(name, value));
        }
    }

    public static void AddOptional(this List<KeyValuePair<string, string>> parameters, string name, int? value)
    {
        if (value != null)
        {
            parameters.Add(new(name, value.Value.ToString(CultureInfo.InvariantCulture)));
        }
    }

    public static void AddOptional(this List<KeyValuePair<string, string>> parameters, string name, double? value)
    {
        if (value != null)
        {
            parameters.Add(new(name, value.Value.ToString(CultureInfo.InvariantCulture)));
        }
    }

    public static void AddOptional(this List<KeyValuePair<string, string>> parameters, string name, bool? value)
    {
        if (value != null)
        {
            parameters.Add(new(name, value.Value ? "true" : "false"));
        }
    }

    public static void AddMembers(this List<KeyValuePair<string, string>> parameters, string name, IReadOnlyList<string>? values)
    {
        if (values is null) return;

        for (int i = 0; i < values.Count; i++)
        {
            parameters.Add(new(Member(name, i), values[i]));
        }
    }

    public static void AddDimensions(this List<KeyValuePair<string, string>> parameters, string name, IReadOnlyList<Dimension>? dimensions)
    {
        if (dimensions is null) return;

        for (int i = 0; i < dimensions.Count; i++)
        {
            string prefix = Member(name, i);

            parameters.Add(new($"{prefix}.Name", dimensions[i].Name));
            parameters.Add(new($"{prefix}.Value", dimensions[i].Value));
        }
    }

    public static string Member(string name, int index)
    {
        return string.Create(CultureInfo.InvariantCulture, $"{name}.member.{index + 1}");
    }
}
