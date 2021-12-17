using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.ApiClient.Models;

[assembly:InternalsVisibleTo("STAIExtensions.Core.Tests")]

namespace STAIExtensions.Core.ApiClient
{
    public class AIQueryApiClient : Abstractions.ApiClient.IAIQueryApiClient
    {

        #region Members
        private const string QueryApiBaseUrl = "https://api.applicationinsights.io/v1/apps/{app-id}/query";
        private string? _queryApiUrl;
        private readonly ILogger<AIQueryApiClient>? _logger;
        #endregion

        #region Properties

        public bool Configured { get; private set; } = false;

        public string AppId { get; private set; } = "";

        public string AppKey { get; private set; } = "";
        #endregion
        
        #region ctor
        public AIQueryApiClient(ILogger<AIQueryApiClient>? logger = default)
        {
            this._logger = logger;
        }
        #endregion

        #region Methods

        public virtual void ConfigureApi(string appId, string appKey)
        {
            if (string.IsNullOrEmpty(appId) || appId.Trim() == "")
                throw new ArgumentNullException(nameof(appId));
            
            if (string.IsNullOrEmpty(appKey) || appKey.Trim() == "")
                throw new ArgumentNullException(nameof(appKey));
            
            this.AppId = appId;
            this.AppKey = appKey;
            
            this._queryApiUrl = QueryApiBaseUrl.Replace("{app-id}", this.AppId);
            
            Configured = true;
        }
        
        public virtual  WebApiResponse ExecuteQuery(string query)
        {
            if (Configured == false)
                throw new InvalidOperationException("Please call ConfigureApi before attempting to execute queries");
            
            return ExecuteQueryAsync(query).GetAwaiter().GetResult();
        }
       
        public virtual  async Task<WebApiResponse> ExecuteQueryAsync(string query)
        {
            if (Configured == false)
                throw new InvalidOperationException("Please call ConfigureApi before attempting to execute queries");

            try
            {
                if (string.IsNullOrEmpty(query) || query.Trim() == "")
                    throw new ArgumentNullException(nameof(query));
                
                using (var httpClient = new HttpClient())
                {
                    
                    this._logger?.LogTrace($"Loading data from Api");
                    httpClient.DefaultRequestHeaders.Add("x-api-key", this.AppKey);

                    var response = await httpClient.PostAsJsonAsync(this._queryApiUrl, new QueryBody(query));
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        this._logger?.LogTrace($"Data returned successfully");
                        return new WebApiResponse(responseData, true);
                    }
                    this._logger?.LogWarning($"Api call does not indicate success. {responseData}");
                    return new WebApiResponse(null, false, $"Response status: {response.StatusCode}. {responseData} ");
                }
            }
            catch (Exception ex)
            {
                this._logger?.LogCritical($"There was an error getting the response from the server. {ex}");
                return new WebApiResponse(null, false, ex.ToString());
            }
        }

        public virtual ApiClientQueryResult ParseResponse(WebApiResponse webApiResponse)
        {
            try
            {
                if (webApiResponse == null)
                    throw new ArgumentNullException(nameof(webApiResponse));
            
                if (webApiResponse.Success == false)
                    throw new Exception("Web Api response was not successful, unable to parse");

                if (string.IsNullOrEmpty(webApiResponse.ResponseData))
                    throw new Exception("Web Api response did not have a valid value");

                var result =
                    System.Text.Json.JsonSerializer.Deserialize<Abstractions.ApiClient.Models.ApiClientQueryResult>(webApiResponse
                        .ResponseData);
            
                return result;
            }
            catch (Exception ex)
            {
                this._logger?.LogError(ex, $"Error parsing response");
                throw;
            }
        }
        #endregion

        #region Helper Classes
        private record struct QueryBody(string query);
        #endregion

    }
}