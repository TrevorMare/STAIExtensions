namespace STAIExtensions.Abstractions.ApiClient;

public interface IAIQueryApiClient
{
    Abstractions.WebApi.WebApiResponse ExecuteQuery(string query);
    
    Task<Abstractions.WebApi.WebApiResponse> ExecuteQueryAsync(string query);

    Models.ApiClientQueryResult ParseResponse(Abstractions.WebApi.WebApiResponse webApiResponse);
}    
    

