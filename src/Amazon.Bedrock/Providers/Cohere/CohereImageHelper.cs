namespace Cohere;

public static class CohereDataUrl
{
    public static string Create(string contentType, scoped ReadOnlySpan<byte> data)
    {
        // The original image file type must be in a png, jpeg, webp, or gif format and can be up to 5 MB in size.

        if (data.Length > 5_000_000)
        {
            throw new ArgumentException("Must not exceed 5,000,000 elements", nameof(data));
        }

        return $"data:{contentType};base64,{Convert.ToBase64String(data)}";
    }
}