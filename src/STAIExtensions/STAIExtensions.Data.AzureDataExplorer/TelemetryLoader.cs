using System.Runtime.CompilerServices;
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

    private readonly ILogger<TelemetryLoader>? _logger;

    private readonly TelemetryLoaderOptions _loaderOptions;

    private readonly AzureDataExplorerClient _azureDataExplorerClient;

    private readonly TableRowDeserializer _tableRowDeserializer;
    #endregion

    #region Properties
    public IDataContractQueryFactory? DataContractQueryFactory { get; set; } =
        new Queries.AzureDataExplorerQueryFactory();
    #endregion

    #region ctor
    public TelemetryLoader(Func<TelemetryLoaderOptions> loaderOptionsBuilder)
        : this()
    {
        if (loaderOptionsBuilder == null)
            throw new ArgumentNullException(nameof(loaderOptionsBuilder));
        
        this._loaderOptions = loaderOptionsBuilder.Invoke();
    }
    
    public TelemetryLoader(TelemetryLoaderOptions loaderOptions)
        : this()
    {
    }

    private TelemetryLoader()
    {
        this._logger = Abstractions.DependencyExtensions.CreateLogger<TelemetryLoader>();
        
        if (this._loaderOptions == null)
            throw new ArgumentNullException(nameof(_loaderOptions));        
     
        this._azureDataExplorerClient = new AzureDataExplorerClient(this._loaderOptions);
        this._tableRowDeserializer = new TableRowDeserializer();
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(IDataContractQuery<T> query) where T : IDataContract
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        if (query.Enabled == false)
            return new List<T>();

        var queryData = query.BuildQueryData()?.ToString() ?? "";
        if (string.IsNullOrEmpty(queryData))
        {
            
        }
        
        
        // Call the telemetry client to load the query
        var clientResponse = await _azureDataExplorerClient.ExecuteQueryAndGetResponseAsync(queryData);
        
        if (clientResponse?.Tables == null)
            return new List<T>();

        if (clientResponse.Tables.Count() != 1)
            throw new Exception("Unexpected number of tables in deserialization");
        
        // Get the row data
        IEnumerable<T> result = this._tableRowDeserializer.DeserializeTableRows<T>(clientResponse.Tables.FirstOrDefault());
         

        // Return the row data for the query

        return result;
    }
    #endregion
    
    
}