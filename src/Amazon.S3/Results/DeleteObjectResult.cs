namespace Amazon.S3
{
    public sealed class DeleteObjectResult
    {
        public DeleteObjectResult(
            string? deleteMarker, 
            string? requestCharged, 
            string? versionId)
        {
            DeleteMarker = deleteMarker;
            RequestCharged = requestCharged;
            VersionId = versionId;
        }

        public string? DeleteMarker { get; }

        public string? VersionId { get; }

        public string? RequestCharged { get; }

        public bool IsDeleteMarker => DeleteMarker is "true";
    }
}

/*
HTTP/1.1 204
x-amz-delete-marker: DeleteMarker
x-amz-version-id: VersionId
x-amz-request-charged: RequestCharged
x-amz-delete-marker: true
*/