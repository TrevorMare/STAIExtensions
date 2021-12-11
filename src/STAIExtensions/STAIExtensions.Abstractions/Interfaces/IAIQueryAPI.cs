namespace STAIExtensions.Abstractions.Interfaces
{
    public interface IAIQueryAPI
    {
        Task<Abstractions.WebApi.WebApiResponse> ExecuteQueryAsync(string query);
    }    
    
}

