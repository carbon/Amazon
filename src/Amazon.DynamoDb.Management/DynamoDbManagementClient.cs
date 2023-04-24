using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

public sealed class DynamoDbManagementClient : AwsClient
{
    private const string TargetPrefix = "DynamoDB_20120810";

    private static readonly JsonSerializerOptions s_serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public DynamoDbManagementClient(AwsRegion region, IAwsCredential credential)
        : base(AwsService.DynamoDb, region, credential)
    {
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public Task<CreateTableResult> CreateTableAsync(CreateTableRequest request)
    {
        return HandleRequestAsync<CreateTableRequest, CreateTableResult>("CreateTable", request);
    }

    public Task<DeleteTableResult> DeleteTableAsync(string tableName)
    {
        return HandleRequestAsync<TableRequest, DeleteTableResult>("DeleteTable", new TableRequest(tableName));
    }

    public Task<DescribeTableResult> DescribeTableAsync(string tableName)
    {
        return HandleRequestAsync<TableRequest, DescribeTableResult>("DescribeTable", new TableRequest(tableName));
    }

    public Task<DescribeTimeToLiveResult> DescribeTimeToLiveAsync(string tableName)
    {
        return HandleRequestAsync<TableRequest, DescribeTimeToLiveResult>("DescribeTimeToLive", new TableRequest(tableName));
    }

    public Task<ListTablesResult> ListTablesAsync(ListTablesRequest request)
    {
        return HandleRequestAsync<ListTablesRequest, ListTablesResult>("ListTables", request);
    }

    public Task<UpdateTableResult> UpdateTableAsync(UpdateTableRequest request)
    {
        return HandleRequestAsync<UpdateTableRequest, UpdateTableResult>("UpdateTable", request);
    }

    public Task<UpdateTimeToLiveResult> UpdateTimeToLiveAsync(UpdateTimeToLiveRequest request)
    {
        return HandleRequestAsync<UpdateTimeToLiveRequest, UpdateTimeToLiveResult>("UpdateTimeToLive", request);
    }

    #region Helpers

    private async Task<TResult> HandleRequestAsync<TRequest, TResult>(string action, TRequest request)
    {
        var httpRequest = Setup(action, JsonSerializer.SerializeToUtf8Bytes(request));

        await SignAsync(httpRequest).ConfigureAwait(false);

        using HttpResponseMessage response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw await DynamoDbException.FromResponseAsync(response).ConfigureAwait(false);
        }


        var result = await response.Content.ReadFromJsonAsync<TResult>(s_serializerOptions).ConfigureAwait(false);

        return result!;
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
