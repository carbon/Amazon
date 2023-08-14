using Amazon;
using Amazon.S3;

namespace Wasabi;

public sealed class WasabiClient(WasabiEndpoint endpoint, IAwsCredential credential) 
    : S3Client(endpoint.Region, endpoint.Host, credential)
{
    public async Task MoveAsync(MoveObjectRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await SendS3RequestAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);        
    }

    // TODO: Append
}