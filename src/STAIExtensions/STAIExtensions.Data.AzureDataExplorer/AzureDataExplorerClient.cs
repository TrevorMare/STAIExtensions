using System.Net.Http.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using STAIExtensions.Data.AzureDataExplorer.Models;

namespace STAIExtensions.Data.AzureDataExplorer
{
    internal class AzureDataExplorerClient 
    {

        #region Members
        private const string QueryApiBaseUrl = "https://api.applicationinsights.io/v1/apps/{app-id}/query";
        private string? _queryApiUrl;
        private readonly ILogger<AzureDataExplorerClient>? _logger;
        private readonly TelemetryClient? _telemetryClient;
        #endregion

        #region Properties
        public bool Configured { get; private set; } 

        public string AppId { get; private set; } = "";

        public string AppKey { get; private set; } = "";
        #endregion
        
        #region ctor
        public AzureDataExplorerClient(TelemetryLoaderOptions options)
        {
            this._logger = Abstractions.DependencyExtensions.CreateLogger<AzureDataExplorerClient>();
            this._telemetryClient = Abstractions.DependencyExtensions.TelemetryClient;
            
            this.ConfigureApi(options.AppId, options.ApiKey);
        }
        #endregion

        #region Methods

        internal ApiClientQueryResult? ExecuteQueryAndGetResponse(string query)
        {
            var queryResponse = ExecuteQuery(query);
            if (queryResponse.Success == false)
                throw new Exception(queryResponse.ErrorMessage);
           
            return ParseResponse(queryResponse);
        }
        
        internal async Task<ApiClientQueryResult?> ExecuteQueryAndGetResponseAsync(string query)
        {
            var queryResponse = await ExecuteQueryAsync(query);
            if (queryResponse.Success == false)
                throw new Exception(queryResponse.ErrorMessage);
           
            return ParseResponse(queryResponse);
        }
        
        internal WebApiResponse ExecuteQuery(string query)
        {
            if (Configured == false)
                throw new InvalidOperationException("Please call ConfigureApi before attempting to execute queries");
            
            this._logger?.LogTrace($"Executing Synchronous Query on {nameof(AzureDataExplorerClient)}");
            
            return ExecuteQueryAsync(query).GetAwaiter().GetResult();
        }
       
        internal async Task<WebApiResponse> ExecuteQueryAsync(string query)
        {
            if (Configured == false)
                throw new InvalidOperationException("Please call ConfigureApi before attempting to execute queries");

            this._logger?.LogTrace($"Executing Asynchronous Query on {nameof(AzureDataExplorerClient)}");

            using var clientExecuteAsyncOperation = this._telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(ExecuteQueryAsync)}");
            
            try
            {
                if (string.IsNullOrEmpty(query) || query.Trim() == "")
                    throw new ArgumentNullException(nameof(query));

                using var httpClient = new HttpClient();
                this._logger?.LogTrace($"Loading data from Api");
                httpClient.DefaultRequestHeaders.Add("x-api-key", this.AppKey);

                var response = await httpClient.PostAsJsonAsync(this._queryApiUrl, new WebApiQueryBody(query));
                var responseData = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    this._logger?.LogTrace($"Data returned successfully");
                    return new WebApiResponse(responseData, true);
                }
                this._logger?.LogWarning("Api call does not indicate success. {ResponseData}", responseData);
                return new WebApiResponse(null, false, $"Response status: {response.StatusCode}. {responseData} ");
            }
            catch (Exception ex)
            {
                if (clientExecuteAsyncOperation != null)
                    clientExecuteAsyncOperation.Telemetry.Success = false;

                Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                    "There was an error getting the response from the server. {ResponseException}", ex);

                return new WebApiResponse(null, false, ex.ToString());
            }
        }

        internal ApiClientQueryResult? ParseResponse(WebApiResponse webApiResponse)
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
                    System.Text.Json.JsonSerializer.Deserialize<ApiClientQueryResult>(webApiResponse
                        .ResponseData);
            
                return result;
            }
            catch (Exception ex)
            {
                Abstractions.Common.ErrorLoggingFactory.LogError(this._telemetryClient, this._logger, ex,
                    $"Error parsing response");
                throw;
            }
        }
        #endregion

        #region Private Methods
        private void ConfigureApi(string appId, string appKey)
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
        #endregion

    }
}