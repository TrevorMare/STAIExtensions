using System;
using System.Net.Http.Json;
using STAIExtensions.Abstractions.WebApi;

namespace STAIExtensions.Core
{
    public class AIQueryAPI : Abstractions.Interfaces.IAIQueryAPI
    {

        #region Members
        private const string _queryApiBaseUrl = "https://api.applicationinsights.io/v1/apps/{app-id}/query";
        private readonly string _queryApiUrl;
        #endregion

        #region Properties
        public string AppId { get; }
        #endregion
        
        #region ctor

        public AIQueryAPI(string appId)
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
        #endregion

        #region Helper Classes

        private record struct QueryBody(string query);
        #endregion

    }
}