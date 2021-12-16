using System.Collections;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.ApiClient;
using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Abstractions.Serialization;

namespace STAIExtensions.Core.Collections;

public abstract class QueryDataSet : Abstractions.Collections.IQueryDataSet
{

    /*
     * Create a new Query Data Set - Done
     * Set the App Id and App Key - Done
     * Add the queries to execute, either custom or predefined - Done
     * Setup the timer interval 
     * Start the dataset auto refresh
     *
     * On refresh, load the data from kusto query - Done
     * Deserialize the data into rows - Done
     * Raise events
     * 
     */

    public event EventHandler OnDatasetUpdated;

    #region Properties

    public IQueryBuilder QueryBuilder => _queryBuilderLazy.Value;

    #endregion
    
    #region Protected Members
    protected readonly ILogger<QueryDataSet>? Logger;

    protected IAIQueryApiClient AIQueryApiClient => _apiClientLazy.Value;

    protected ITableRowDeserializer TableRowDeserializer => _tableRowDeserializerLazy.Value;

    // Do a shallow clone of the list just to avoid threading issues
    protected List<IDataContractQuery> Queries => new List<IDataContractQuery>(_queries);

    protected List<IDataContractQuery> EnabledQueries => Queries.Where(x => x.Enabled == true).ToList();
    #endregion

    #region Private Members

    private readonly Lazy<IAIQueryApiClient> _apiClientLazy;

    private readonly Lazy<ITableRowDeserializer> _tableRowDeserializerLazy;
    
    private readonly Lazy<IQueryBuilder> _queryBuilderLazy;
    
    private readonly SynchronizedCollection<Abstractions.Queries.IDataContractQuery> _queries = new SynchronizedCollection<IDataContractQuery>();
    #endregion

    #region Properties
    public string DataSetId { get; set; } = Guid.NewGuid().ToString();
    
    public string AppId { get; private set; } = "";

    public string AppKey { get; private set; } = "";
    
    public bool Configured { get; private set; } = false;

    #endregion

    #region ctor

    public QueryDataSet(IServiceProvider serviceProvider, ILogger<QueryDataSet>? logger = default)
    {
        
        // Initialise the client API
        this._apiClientLazy = new Lazy<IAIQueryApiClient>(() =>
        {
            if (Configured == true)
                throw new Exception("Configure the Api service first before attempting to load");
            
            var instance = serviceProvider.GetRequiredService<IAIQueryApiClient>();
            instance.ConfigureApi(AppId, AppKey);
            return instance;
        });

        // Initialise the table row deserializer
        this._tableRowDeserializerLazy =
            new Lazy<ITableRowDeserializer>(serviceProvider.GetRequiredService<ITableRowDeserializer>);
        
        // Initialise the query builder
        this._queryBuilderLazy =
            new Lazy<IQueryBuilder>(serviceProvider.GetRequiredService<IQueryBuilder>);
        
        this.Logger = logger;
    }
    #endregion

    #region Public Methods

    public virtual void SetupApiConfig(string appId, string appKey)
    {
        if (string.IsNullOrEmpty(appId) || appId.Trim() == "")
            throw new ArgumentNullException(nameof(appId));
            
        if (string.IsNullOrEmpty(appKey) || appKey.Trim() == "")
            throw new ArgumentNullException(nameof(appKey));

        this.AppId = appId;
        this.AppKey = appKey;
        
        this.Logger?.LogTrace("Configuring dataset with id {DataSetId} for app Id {AppId}", DataSetId, AppId );
        
        this.Configured = true;
    }

    public virtual void AddDataQuery(Abstractions.Queries.IDataContractQuery dataContractQuery)
    {
        if (dataContractQuery == null)
            throw new ArgumentNullException(nameof(dataContractQuery));
        
        this._queries.Add(dataContractQuery);
    }
    
    public virtual void AddDataQueries(IEnumerable<Abstractions.Queries.IDataContractQuery> dataContractQueries)
    {
        if (dataContractQueries == null)
            throw new ArgumentNullException(nameof(dataContractQueries));

        var contractQueries = dataContractQueries as IDataContractQuery[] ?? dataContractQueries.ToArray();
        contractQueries.ToList().ForEach(this.AddDataQuery);
    }

    public virtual void RemoveDataQuery(Abstractions.Queries.IDataContractQuery dataContractQuery)
    {
        if (dataContractQuery == null)
            throw new ArgumentNullException(nameof(dataContractQuery));
        
        this._queries.Remove(dataContractQuery);
    }

    public virtual void ClearDataQueries()
    {
        this._queries.Clear();
    }

    public virtual void RefreshDataSet()
    {
        try
        {
            this.Logger?.LogTrace($"Starting refresh on dataset {DataSetId}.");
            var enabledQueries = EnabledQueries;
            
            
            var kustoQuery = this.GetKustoQueryToExecute(enabledQueries);
            if (!string.IsNullOrEmpty(kustoQuery))
            {
                var resultSet = this._apiClientLazy.Value.ExecuteQuery(kustoQuery);
                ParseResponseAndBuildResults(resultSet, enabledQueries);
            }
            
            this.Logger?.LogTrace($"Refresh on dataset {DataSetId} completed.");
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, $"Failed to refresh dataset with Id {DataSetId}");
        }
    }
    
    public virtual async Task RefreshDataSetAsync()
    {
        try
        {
            this.Logger?.LogTrace($"Starting refresh on dataset {DataSetId}.");
            var enabledQueries = EnabledQueries;
            
            var kustoQuery = this.GetKustoQueryToExecute(enabledQueries);
            if (!string.IsNullOrEmpty(kustoQuery))
            {
                var resultSet = await this._apiClientLazy.Value.ExecuteQueryAsync(kustoQuery);
                ParseResponseAndBuildResults(resultSet, enabledQueries);
            }
            this.Logger?.LogTrace($"Refresh on dataset {DataSetId} completed.");
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, $"Failed to refresh dataset with Id {DataSetId}");
        }
    }
    #endregion

    #region Protected Methods
    protected virtual void ParseResponseAndBuildResults(WebApiResponse webApiResponse, IEnumerable<IDataContractQuery> queries)
    {
        if (webApiResponse.Success == true)
        {
            var parsedData = this.AIQueryApiClient.ParseResponse(webApiResponse);

            foreach (var query in queries)
            {
                var table = parsedData?.Tables?.FirstOrDefault(x => x.TableName.ToLower() == query.Alias.ToLower());

                if (table != null)
                {
                    DeserializeRows(table, query);
                }
            }
        }
        this.OnDatasetUpdated?.Invoke(this, EventArgs.Empty);
    }

    protected abstract void DeserializeRows(ApiClientQueryResultTable table, IDataContractQuery query);
    
    protected virtual string GetKustoQueryToExecute(IEnumerable<IDataContractQuery> queries)
    {
        var kustoQueryBuilder = new StringBuilder();
        foreach (var query in queries)
        {
            kustoQueryBuilder.AppendLine(query.BuildKustoQuery());
        }

        return kustoQueryBuilder.ToString();
    }
    #endregion


}