namespace Amazon.CloudFront;

public sealed class Invalidation
{
	public required string Id { get; set; }

	public required string Status { get; set; }
}