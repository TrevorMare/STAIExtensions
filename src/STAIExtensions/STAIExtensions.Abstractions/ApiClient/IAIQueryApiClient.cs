using STAIExtensions.Abstractions.ApiClient.Models;

namespace STAIExtensions.Abstractions.ApiClient;

public interface IAIQueryApiClient
{

    #region Properties

    bool Configured { get; }
        
    string AppId { get; }
        
    string AppKey { get; }

    #endregion

    #region Methods
    WebApiResponse ExecuteQuery(string query);
    
    Task<WebApiResponse> ExecuteQueryAsync(string query);

    Models.ApiClientQueryResult ParseResponse(WebApiResponse webApiResponse);

    void ConfigureApi(string appId, string appKey);
    #endregion
    
}    
    

