namespace STAIExtensions.Abstractions.Interfaces;

public interface IAIQueryApi
{
    Abstractions.WebApi.WebApiResponse ExecuteQuery(string query);
    Task<Abstractions.WebApi.WebApiResponse> ExecuteQueryAsync(string query);
}    
    

