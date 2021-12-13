using STAIExtensions.Abstractions.ApiClient.Models;

namespace STAIExtensions.Abstractions.ApiClient;

public interface IAIQueryApiClient
{
    WebApiResponse ExecuteQuery(string query);
    
    Task<WebApiResponse> ExecuteQueryAsync(string query);

    Models.ApiClientQueryResult ParseResponse(WebApiResponse webApiResponse);
}    
    

