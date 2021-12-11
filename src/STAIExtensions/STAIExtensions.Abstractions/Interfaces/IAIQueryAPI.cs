namespace STAIExtensions.Abstractions.Interfaces
{
    public interface IAIQueryAPI
    {
        Abstractions.WebApi.WebApiResponse ExecuteQuery(string query);
        Task<Abstractions.WebApi.WebApiResponse> ExecuteQueryAsync(string query);
    }    
    
}

