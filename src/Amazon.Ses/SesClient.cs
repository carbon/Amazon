using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.Scheduling;

namespace Amazon.Ses;

public sealed class SesClient(AwsRegion region, IAwsCredential credential)
    : AwsClient(new("ses"), region, credential)
{
    private static readonly ExponentialBackoffRetryPolicy s_retryPolicy = new(
        initialDelay: TimeSpan.FromSeconds(1),
        maxDelay: TimeSpan.FromSeconds(10),
        maxRetries: 5
    );

    public Task<SendEmailResult> SendEmailAsync(MailMessage message)
    {
        return SendEmailAsync(SesEmail.FromMailMessage(message));
    }

    public Task<SendEmailResult> SendEmailAsync(SesEmail message)
    {
        return SendEmailAsync(SendEmailRequest.FromSesEmail(message));
    }

    public async Task<SendEmailResult> SendEmailAsync(SendEmailRequest request)
    {
        // https://docs.aws.amazon.com/ses/latest/APIReference-V2/API_SendEmail.html

        return await PostAsync<SendEmailRequest, SendEmailResult>("SendEmail", "/v2/email/outbound-emails", request);
    }

    public async Task<GetAccountResult> GetAccountAsync()
    {
        return await GetAsync<GetAccountResult>("GetAccount", "/v2/email/account");
    }

    private async Task<TResult> PostAsync<T, TResult>([ConstantExpected] string actionName, string path, T request)
    {
        var retryCount = 0;
        Exception lastException;

        do
        {
            if (retryCount > 0)
            {
                await Task.Delay(s_retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"https://email.{Region.Name}.amazonaws.com" + path) {
                    Headers = {
                        { "x-amz-target", "SESv2." + actionName }
                    },
                    Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(request)) {
                        Headers = {
                            { "Content-Type", "application/json" }
                        }
                    }
                };

                var result = await base.SendAsync(httpRequest);

                return JsonSerializer.Deserialize<TResult>(result)!;


            }
            catch (SesException ex) when (ex.IsTransient)
            {
                lastException = ex;
            }

            retryCount++;
        }
        while (s_retryPolicy.ShouldRetry(retryCount));

        throw lastException;       
    }

    private async Task<TResult> GetAsync<TResult>([ConstantExpected] string actionName, string path)
    {        
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://email.{Region.Name}.amazonaws.com" + path) {
            Headers = {
                { "x-amz-target", "SESv2." + actionName }
            }
        };

        var result = await base.SendAsync(httpRequest);

        return JsonSerializer.Deserialize<TResult>(result)!;
    }

    protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
    {
        using (response)
        {
            var text = await response.Content.ReadAsStringAsync();

            if (response.Content.Headers.GetValues("Content-Type").FirstOrDefault() is "application/json")
            {
                var error = JsonSerializer.Deserialize<SesError>(text);

                throw new SesException(error?.Message ?? "Something went wrong", response.StatusCode);
            }
            else
            {
                throw new SesException(text, response.StatusCode);
            }
        }
    }
}

// https://docs.aws.amazon.com/ses/latest/APIReference-V2/Welcome.html