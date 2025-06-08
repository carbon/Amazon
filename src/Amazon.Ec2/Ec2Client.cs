using System.Net.Http;

using Amazon.Ec2.Exceptions;
using Amazon.Ec2.Responses;

namespace Amazon.Ec2;

public sealed class Ec2Client(AwsRegion region, IAwsCredential credential) 
    : AwsClient(AwsService.Ec2, region, credential)
{
    public const string Version = "2016-11-15";
    public const string Namespace = "http://ec2.amazonaws.com/doc/2016-11-15/";

    #region Shortcuts

    public async Task<Image?> DescribeImageAsync(string imageId)
    {
        var result = await DescribeImagesAsync(new DescribeImagesRequest([imageId]));

        return result.Images.Length > 0 ? result.Images[0] : null;
    }

    public async Task<Subnet?> DescribeSubnetAsync(string subnetId)
    {
        var result = await DescribeSubnetsAsync(new DescribeSubnetsRequest([subnetId]));

        return result.Subnets.Length > 0 ? result.Subnets[0] : null;
    }

    public async Task<NetworkInterface?> DescribeNetworkInterfaceAsync(string networkInterfaceId)
    {
        var result = await DescribeNetworkInterfacesAsync(new DescribeNetworkInterfacesRequest([networkInterfaceId]));

        return result.NetworkInterfaces.Length > 0 ? result.NetworkInterfaces[0] : null;
    }

    public async Task<Instance?> DescribeInstanceAsync(string instanceId)
    {
        var result = await DescribeInstancesAsync(new DescribeInstancesRequest([instanceId]));

        return result.Instances.Count > 0 ? result.Instances[0] : null;
    }

    public async Task<InstanceTypeInfo?> DescribeInstanceTypeAsync(string instanceType)
    {
        var result = await DescribeInstanceTypesAsync(new DescribeInstanceTypesRequest(instanceType));

        return result.InstanceTypes.Length > 0 ? result.InstanceTypes[0] : null;
    }
   
    public async Task<Volume?> DescribeVolumeAsync(string volumeId)
    {
        var result = await DescribeVolumesAsync(new DescribeVolumesRequest([volumeId]));

        return result.Volumes.Length > 0 ? result.Volumes[0] : null;
    }

    public async Task<Vpc?> DescribeVpcAsync(string vpcId)
    {
        var result = await DescribeVpcsAsync(new DescribeVpcsRequest([vpcId]));

        return result.Vpcs.Length > 0 ? result.Vpcs[0] : null;
    }

    #endregion

    #region Instances

    public Task<DescribeInstancesResponse> DescribeInstancesAsync(DescribeInstancesRequest request)
    {
        return SendAsync<DescribeInstancesResponse>(request);
    }

    public Task<RunInstancesResponse> RunInstancesAsync(RunInstancesRequest request)
    {
        return SendAsync<RunInstancesResponse>(request);
    }
        
    public Task<RebootInstancesResponse> RebootInstancesAsync(RebootInstancesRequest request)
    {
        return SendAsync<RebootInstancesResponse>(request);
    }

    public Task<StartInstancesResponse> StartInstancesAsync(StartInstancesRequest request)
    {
        return SendAsync<StartInstancesResponse>(request);
    }

    public Task<StopInstancesResponse> StopInstancesAsync(StopInstancesRequest request)
    {
        return SendAsync<StopInstancesResponse>(request);
    }

    public Task<TerminateInstancesResponse> TerminateInstancesAsync(TerminateInstancesRequest request)
    {
        return SendAsync<TerminateInstancesResponse>(request);
    }

    #endregion

    #region Instance Types

    public async Task<DescribeInstanceTypesResponse> DescribeInstanceTypesAsync(DescribeInstanceTypesRequest request)
    {
        byte[] responseBytes = await SendAsync(request).ConfigureAwait(false);

        return Ec2Serializer<DescribeInstanceTypesResponse>.Deserialize(responseBytes);
    }

    #endregion

    #region Images

    public Task<DescribeImagesResponse> DescribeImagesAsync(DescribeImagesRequest request)
    {
        return SendAsync<DescribeImagesResponse>(request);
    }

    public async Task<List<Image>> DescribeAllImagesAsync(DescribeImagesRequest request)
    {
        request.MaxResults = 1000;

        var images = new List<Image>();
        int requestCount = 0;
        DescribeImagesResponse response;

        do
        {
            response = await SendAsync<DescribeImagesResponse>(request);

            requestCount++;

            images.AddRange(response.Images);

            request.NextToken = response.NextToken;
        }
        while (response.NextToken != null && requestCount < 20);

        return images;
    }

    #endregion

    #region Network Interfaces

    public Task<DescribeNetworkInterfacesResponse> DescribeNetworkInterfacesAsync(DescribeNetworkInterfacesRequest request)
    {
        return SendAsync<DescribeNetworkInterfacesResponse>(request);
    }

    #endregion

    #region Subnets

    public Task<DescribeSubnetsResponse> DescribeSubnetsAsync(DescribeSubnetsRequest request)
    {
        return SendAsync<DescribeSubnetsResponse>(request);
    }

    #endregion

    #region Vps

    public Task<DescribeVpcsResponse> DescribeVpcsAsync(DescribeVpcsRequest request)
    {
        return SendAsync<DescribeVpcsResponse>(request);
    }

    #endregion

    #region Volumes

    public Task<DescribeVolumesResponse> DescribeVolumesAsync(DescribeVolumesRequest request)
    {
        return SendAsync<DescribeVolumesResponse>(request);
    }

    #endregion

    #region API Helpers

    private async Task<byte[]> SendAsync(IEc2Request request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParams())
        };

        return await SendAsync(httpRequest).ConfigureAwait(false);
    }

    private async Task<TResponse> SendAsync<TResponse>(IEc2Request request)
        where TResponse: IEc2Response
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Content = GetPostContent(request.ToParams())
        };

        byte[] responseXml = await SendAsync(httpRequest).ConfigureAwait(false);

        return Ec2Serializer<TResponse>.Deserialize(responseXml);
    }

    private static FormUrlEncodedContent GetPostContent(List<KeyValuePair<string, string>> parameters)
    {
        parameters.Add(new ("Version", Version));

        return new FormUrlEncodedContent(parameters!);
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        try
        {
            var errorResponse = ErrorResponse.Deserialize(responseText);

            throw new Ec2Exception(errorResponse.Errors, response.StatusCode);            
        }
        catch { }

        throw new Ec2Exception(responseText, response.StatusCode);
    }

    #endregion
}

// http://docs.aws.amazon.com/AWSEC2/latest/APIReference/Welcome.html
