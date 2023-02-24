using Amazon;
using Amazon.S3;

namespace Wasabi;

public sealed class WasabiClient : S3Client
{
    public WasabiClient(WasabiEndpoint endpoint, IAwsCredential credential)
        : base(endpoint.Region, endpoint.Host, credential) { }


    public async Task MoveAsync(MoveObjectRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await SendS3RequestAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);        
    }

    // TODO: Append
}