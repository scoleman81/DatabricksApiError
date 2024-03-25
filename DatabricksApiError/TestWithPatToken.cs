using Azure.Core;
using Azure.Identity;
using DatabricksApiError.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DatabricksApiError;
public class ConnectionTesting
{
    private string _databricksWorkspace;
    private string _databricksPatToken;
    private HttpClient _httpClient = new HttpClient();
    public ConnectionTesting(string databricksWorkspace, string databricksPatToken)
    {
        _databricksWorkspace = databricksWorkspace;
        _databricksPatToken = databricksPatToken;
        _httpClient.BaseAddress = new Uri(_databricksWorkspace);
    }

    public async Task TestWithPat()
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _databricksPatToken);

        await SendRequests("PAT");
    }

    public async Task TestWithAzureCredentials()
    {
        // I am doing this to speed up response time so it does not have to check everything
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ExcludeEnvironmentCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeWorkloadIdentityCredential = true,
            ExcludeAzureCliCredential = false,
            ExcludeManagedIdentityCredential = true
        });

        //_tokenResponse = await credential.GetTokenAsync(new TokenRequestContext(new[] { "2ff814a6-3304-4ab8-85cb-cd0e6f879c1d/.default" }));
        AccessToken tokenResponse = await credential.GetTokenAsync(new TokenRequestContext(new[] { "2ff814a6-3304-4ab8-85cb-cd0e6f879c1d" }));
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenResponse.Token);

        await SendRequests("AzureCredentials");
    }

    private async Task SendRequests(string typeOfToken)
    {
        Console.WriteLine("==================================");
        Console.WriteLine($"======= {typeOfToken} ===========");
        Console.WriteLine("==================================");
        var request1 = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_databricksWorkspace}/api/2.0/clusters/list")
        };

        var response1 = await _httpClient.SendAsync(request1);
        var content1 = await response1.Content.ReadAsStringAsync();
        Console.WriteLine("======= THIS WILL WORK FOR BOTH TYPES ===========");
        Console.WriteLine(content1);


        var x = JsonSerializer.Serialize(GetLlmMessage());
        var content = new StringContent(JsonSerializer.Serialize(GetLlmMessage()), Encoding.UTF8, new MediaTypeHeaderValue("application/json"));


        var request2 = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_databricksWorkspace}/serving-endpoints/databricks-llama-2-70b-chat/invocations"),
            Content = content
        };

        var response2 = await _httpClient.SendAsync(request2);
        var content2 = await response2.Content.ReadAsStringAsync();
        Console.WriteLine("======= THIS WILL WORK ONLY FOR PAT ===========");
        Console.WriteLine(content2);
    }


    private async Task GetClusterInfo()
    {
        await Task.CompletedTask;
    }

    private DatabricksLlmRequest GetLlmMessage()
    {
        var request = new DatabricksLlmRequest();

        request.Messages.Add(new DatabricksLlmRequestMessage { Role = "user", Content = "What is Databricks?" });
        request.MaxTokens = 128;

        var message = new DatabricksLlmRequestMessage { Role = "user", Content = "What is Databricks?" };
        return request;
    }
}
