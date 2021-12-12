using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using STAIExtensions.Abstractions.WebApi;

[assembly:InternalsVisibleTo("STAIExtensions.Core.Tests")]

namespace STAIExtensions.Core.ApiClient
{
    internal class AIQueryApiClient : Abstractions.ApiClient.IAIQueryApiClient
    {

        #region Members
        private const string _queryApiBaseUrl = "https://api.applicationinsights.io/v1/apps/{app-id}/query";
        private readonly string _queryApiUrl;
        #endregion

        #region Properties
        public string AppId { get; }
        #endregion
        
        #region ctor

        public AIQueryApiClient(string appId)
        {
            if (string.IsNullOrEmpty(appId) || appId.Trim() == "")
                throw new ArgumentNullException(nameof(appId));
            this.AppId = appId;
            this._queryApiUrl = _queryApiBaseUrl.Replace("{app-id}", this.AppId);
        }
        #endregion

        #region Methods
        public Abstractions.WebApi.WebApiResponse ExecuteQuery(string query)
        {
            return ExecuteQueryAsync(query).GetAwaiter().GetResult();
        }
       
        public async Task<Abstractions.WebApi.WebApiResponse> ExecuteQueryAsync(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query) || query.Trim() == "")
                    throw new ArgumentNullException(nameof(query));
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8,");

                    var response = await httpClient.PostAsJsonAsync(this._queryApiUrl, new QueryBody(query));
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return new WebApiResponse(responseData, true);
                    }
                    
                    return new WebApiResponse(null, false, $"Response status: {response.StatusCode}. {responseData} ");
                }
            }
            catch (Exception ex)
            {
                return new WebApiResponse(null, false, ex.ToString());
            }
        }

        public Abstractions.ApiClient.Models.ApiClientQueryResult ParseResponse(Abstractions.WebApi.WebApiResponse webApiResponse)
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
        #endregion

        #region Helper Classes
        private record struct QueryBody(string query);
        #endregion

    }
}