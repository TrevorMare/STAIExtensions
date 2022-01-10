﻿using System.Runtime.CompilerServices;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Data.AzureDataExplorer.Queries;
using STAIExtensions.Data.AzureDataExplorer.Serialization;

[assembly:InternalsVisibleTo("STAIExtensions.Data.AzureDataExplorer.Tests")]


namespace STAIExtensions.Data.AzureDataExplorer;

public class TelemetryLoader : Abstractions.Data.ITelemetryLoader
{

    #region Members

    private ILogger<TelemetryLoader>? _logger;

    private TelemetryLoaderOptions _loaderOptions;

    private AzureDataExplorerClient _azureDataExplorerClient;

    private TableRowDeserializer _tableRowDeserializer;

    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region Properties
    public IDataContractQueryFactory? DataContractQueryFactory { get; set; } = null;
    #endregion

    #region ctor
    public TelemetryLoader(Func<TelemetryLoaderOptions> loaderOptionsBuilder)
    {
        if (loaderOptionsBuilder == null)
            throw new ArgumentNullException(nameof(loaderOptionsBuilder));
        
        this._loaderOptions = loaderOptionsBuilder.Invoke();
        this._telemetryClient =
            (TelemetryClient?) Abstractions.DependencyExtensions.ServiceProvider?.GetService(typeof(TelemetryClient));
        
        Initialise();
    }
    
    public TelemetryLoader(TelemetryLoaderOptions loaderOptions)
    {
        _loaderOptions = loaderOptions;
        Initialise();
    }

    #endregion

    #region Initialise
    private void Initialise()
    {
        this.DataContractQueryFactory = new Queries.AzureDataExplorerQueryFactory();
        
        this._logger = Abstractions.DependencyExtensions.CreateLogger<TelemetryLoader>();
        
        if (this._loaderOptions == null)
            throw new ArgumentNullException(nameof(_loaderOptions));        
     
        this._azureDataExplorerClient = new AzureDataExplorerClient(this._loaderOptions);
        this._tableRowDeserializer = new TableRowDeserializer();
    }
    #endregion
    
    #region Methods
   
    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(DataContractQuery<T> query) where T : Abstractions.DataContracts.Models.DataContract
    {

        using var executeOperation = this._telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(ExecuteQueryAsync)}");

        try
        {
            this._logger?.LogTrace("Start Execute Query on Azure Data Explorer");

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (query.Enabled == false)
            {
                this._logger?.LogTrace("Query disabled");
                return new List<T>();
            }

            var queryData = query.BuildQueryData()?.ToString() ?? "";
            
            if (executeOperation != null)
                executeOperation.Telemetry.Data = queryData;
            
            if (string.IsNullOrEmpty(queryData))
            {
                this._logger?.LogWarning("Query did not return any data to retrieve");
                return new List<T>();
            }
        
            // Call the telemetry client to load the query
            var clientResponse = await _azureDataExplorerClient.ExecuteQueryAndGetResponseAsync(queryData);
        
            if (clientResponse?.Tables == null)
                return new List<T>();

            if (clientResponse.Tables.Count() != 1)
                throw new Exception("Unexpected number of tables in deserialization");
        
            IEnumerable<T> result = this._tableRowDeserializer.DeserializeTableRows<T>(clientResponse.Tables.FirstOrDefault());

            return result;
        }
        catch (Exception ex)
        {
            if (executeOperation != null)
                executeOperation.Telemetry.Success = false;
            
            this._telemetryClient?.TrackException(ex);
            
            this._logger?.LogError(ex,
                "An error occured executing query in Azure Data Explorer Telemetry loader: {ErrorMessage}", ex.Message);
            throw;
        }
        
    }
    #endregion
    
}