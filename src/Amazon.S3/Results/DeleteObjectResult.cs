namespace Amazon.S3;

public sealed class DeleteObjectResult(
    string? deleteMarker,
    string? requestCharged,
    string? versionId)
{
    public string? DeleteMarker { get; } = deleteMarker;

    public string? VersionId { get; } = versionId;

    public string? RequestCharged { get; } = requestCharged;

    public bool IsDeleteMarker => DeleteMarker is "true";
}

/*
HTTP/1.1 204
x-amz-delete-marker: DeleteMarker
x-amz-version-id: VersionId
x-amz-request-charged: RequestCharged
x-amz-delete-marker: true
*/