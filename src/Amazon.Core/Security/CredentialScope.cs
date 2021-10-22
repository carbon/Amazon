using System;
using System.Globalization;
using System.Text;

namespace Amazon.Security;

public readonly struct CredentialScope : ISpanFormattable
{
    public CredentialScope(DateTime date, AwsRegion region, AwsService service)
    {
        Date = date;
        Region = region ?? throw new ArgumentNullException(nameof(region));
        Service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public DateTime Date { get; }

    public AwsRegion Region { get; }

    public AwsService Service { get; }

    // 20120228/us-east-1/iam/aws4_request
    public readonly override string ToString() => $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request";

    private const string dateFormat = "yyyyMMdd";

    internal void FormatDateTo(Span<byte> utf8Destination)
    {
        Span<char> formattedDateChars = stackalloc char[8];

        Date.TryFormat(formattedDateChars, out _, dateFormat, CultureInfo.InvariantCulture);

        Encoding.ASCII.GetBytes(formattedDateChars, utf8Destination);
    }

    internal void FormatDateTo(Span<char> utf16Destination)
    {
        Date.TryFormat(utf16Destination, out _, dateFormat, CultureInfo.InvariantCulture);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(CultureInfo.InvariantCulture, $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request");
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite(CultureInfo.InvariantCulture, $"{Date:yyyyMMdd}/{Region}/{Service}/aws4_request", out charsWritten);
    }

    internal readonly void AppendTo(ref ValueStringBuilder output)
    {
        FormatDateTo(output.AppendSpan(8));
        output.Append('/');
        output.Append(Region.Name);
        output.Append('/');
        output.Append(Service.Name);
        output.Append('/');
        output.Append("aws4_request");
    }
}