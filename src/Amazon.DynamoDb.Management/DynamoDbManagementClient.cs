using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amazon.DynamoDb;

public sealed class DynamoDbManagementClient : AwsClient
{
    private const string TargetPrefix = "DynamoDB_20120810";

    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public DynamoDbManagementClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.DynamoDb, region, credential)
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<CreateTableResult> CreateTableAsync(CreateTableRequest request)
    {
        return await HandleRequestAsync<CreateTableRequest, CreateTableResult>("CreateTable", request).ConfigureAwait(false);
    }

    public async Task<DeleteTableResult> DeleteTableAsync(string tableName)
    {
        return await HandleRequestAsync<TableRequest, DeleteTableResult>("DeleteTable", new TableRequest(tableName)).ConfigureAwait(false);
    }

    public async Task<DescribeTableResult> DescribeTableAsync(string tableName)
    {
        return await HandleRequestAsync<TableRequest, DescribeTableResult>("DescribeTable", new TableRequest(tableName)).ConfigureAwait(false);
    }

    public async Task<DescribeTimeToLiveResult> DescribeTimeToLiveAsync(string tableName)
    {
        return await HandleRequestAsync<TableRequest, DescribeTimeToLiveResult>("DescribeTimeToLive", new TableRequest(tableName)).ConfigureAwait(false);
    }

    public async Task<ListTablesResult> ListTablesAsync(ListTablesRequest request)
    {
        return await HandleRequestAsync<ListTablesRequest, ListTablesResult>("ListTables", request).ConfigureAwait(false);
    }

    public async Task<UpdateTableResult> UpdateTableAsync(UpdateTableRequest request)
    {
        return await HandleRequestAsync<UpdateTableRequest, UpdateTableResult>("UpdateTable", request).ConfigureAwait(false);
    }

    public async Task<UpdateTimeToLiveResult> UpdateTimeToLiveAsync(UpdateTimeToLiveRequest request)
    {
        return await HandleRequestAsync<UpdateTimeToLiveRequest, UpdateTimeToLiveResult>("UpdateTimeToLive", request).ConfigureAwait(false);
    }

    #region Helpers

    [AsyncMethodBuilder(typeof(PoolingAsyncValueTaskMethodBuilder<>))]
    private async ValueTask<TResult> HandleRequestAsync<TRequest, TResult>(string action, TRequest request)
    {
        var httpRequest = Setup(action, JsonSerializer.SerializeToUtf8Bytes(request));

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await DynamoDbException.FromResponseAsync(response).ConfigureAwait(false);
        }

        using Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

        return (await JsonSerializer.DeserializeAsync<TResult>(stream, serializerOptions).ConfigureAwait(false))!;
    }

    private HttpRequestMessage Setup(string action, byte[]? utf8Json)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint) {
            Headers = {
                { "Accept-Encoding", "gzip" },
                { "x-amz-target", $"{TargetPrefix}.{action}" }
            }
        };

        if (utf8Json != null)
        {
            request.Content = new ByteArrayContent(utf8Json)
            {
                Headers = {
                    { "Content-Type", "application/x-amz-json-1.0" }
                }
            };
        }

        return request;
    }

    #endregion
}
