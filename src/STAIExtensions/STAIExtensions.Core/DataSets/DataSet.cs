using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.DataSets;

/// <summary>
/// Abstract implementation of the IDataSet 
/// </summary>
public abstract class DataSet : Abstractions.Data.IDataSet
{

    #region Members

    private bool _disposed = false;

    protected readonly TelemetryClient? TelemetryClient;

    #endregion
    
    #region Properties
    /// <summary>
    /// Gets or sets the Data Set Friendly name
    /// </summary>
    public string DataSetName { get; set; }

    /// <summary>
    /// Gets or sets the unique Id of the DataSet
    /// </summary>
    public string DataSetId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the fully qualified type name of the DataSet
    /// </summary>
    public string DataSetType => this.GetType().AssemblyQualifiedName;
    
    /// <summary>
    /// Gets the fully qualified type name of the DataSet
    /// </summary>
    public string FriendlyDataSetType => this.GetType().Name;

    /// <summary>
    /// An Event that occurs once all queries has run and the dataset has updated
    /// </summary>
    public event EventHandler? OnDataSetUpdated;
    
    /// <summary>
    /// Gets a value indicating if the DataSet is currently Auto-Refreshing
    /// </summary>
    public bool AutoRefreshEnabled { get; protected set; }

    /// <summary>
    /// Gets the Cancellation Token for the Data Set
    /// </summary>
    protected CancellationToken? CancellationToken { get; private set; }
    
    /// <summary>
    /// Gets the default Logger for the Data Set if registered
    /// </summary>
    protected ILogger<DataSet>? Logger { get; private set; }
    
    /// <summary>
    /// Gets the Auto Refresh interval from the last Start Auto Refresh value
    /// </summary>
    protected TimeSpan? AutoRefreshInterval { get; private set; }
    #endregion
    
    #region Members
    protected readonly Abstractions.Data.ITelemetryLoader TelemetryLoader;
    
    private readonly Timer _autoRefreshTimer;
    #endregion

    #region ctor
    protected DataSet(Abstractions.Data.ITelemetryLoader telemetryLoader, string? dataSetName = default)
    {
        this.TelemetryLoader = telemetryLoader ?? throw new ArgumentNullException(nameof(telemetryLoader));
        this.Logger = Abstractions.DependencyExtensions.CreateLogger<DataSet>();
        this._autoRefreshTimer = new Timer(OnTimerTick);
        this.DataSetName = string.IsNullOrEmpty(dataSetName) ? Guid.NewGuid().ToString() : dataSetName;
        this.TelemetryClient = Abstractions.DependencyExtensions.TelemetryClient;
       
        // Attach this data set to the data set collection
        Abstractions.DependencyExtensions.Mediator?.Send(new Abstractions.CQRS.DataSets.Commands.AttachDataSetCommand(this));
    }
    #endregion

    #region Methods
    /// <summary>
    /// Starts the Auto Refresh process
    /// </summary>
    /// <param name="autoRefreshInterval">The Auto Refresh Interval</param>
    /// <param name="cancellationToken">A Cancellation Token to stop the processing with</param>
    /// <exception cref="ArgumentException"></exception>
    public void StartAutoRefresh(TimeSpan autoRefreshInterval, CancellationToken? cancellationToken = default)
    {
        if (autoRefreshInterval.Ticks < 0)
            throw new ArgumentException(nameof(autoRefreshInterval));
        
        this.AutoRefreshInterval = autoRefreshInterval;
        this.CancellationToken = cancellationToken;
        this.AutoRefreshEnabled = true;
        
        this.Logger?.LogInformation("Starting auto refresh on dataset {DataSetName} with interval {Interval}", DataSetName, AutoRefreshInterval);
        this._autoRefreshTimer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
    }

    /// <summary>
    /// Stops the DataSet Auto-Refresh processing
    /// </summary>
    public void StopAutoRefresh()
    {
        this.Logger?.LogInformation("Stopping auto refresh on dataset {DataSetName}", DataSetName);
        this.AutoRefreshEnabled = false;
        this._autoRefreshTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    /// <summary>
    /// Execute the Data Set queries and handle the response of the queries
    /// </summary>
    /// <returns></returns>
    protected abstract Task ExecuteQueries();

    /// <summary>
    /// Executes a query against the telemetry loader instance
    /// </summary>
    /// <param name="query">The query to execute</param>
    /// <typeparam name="T">The return record type</typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual async Task ExecuteDataQuery<T>(DataContractQuery<T> query) where T : Abstractions.DataContracts.Models.DataContract
    {
        
        using var loadDataOperation = this.TelemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(ExecuteDataQuery)}"); 
        
        try
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
           
            var queryResult = await TelemetryLoader.ExecuteQueryAsync(query);

            this.Logger?.LogInformation("Query type {TypeName} returned {NumberOfRows} rows", query.ContractType.Name,
                queryResult?.Count() ?? 0);
            
            if (loadDataOperation != null)
            {
                loadDataOperation.Telemetry.Properties["DataSetType"] = this.DataSetType;
                loadDataOperation.Telemetry.Properties["DataSetName"] = this.DataSetName;
                loadDataOperation.Telemetry.Properties["QueryType"] = query.ContractType.Name;
                loadDataOperation.Telemetry.Properties["RowsReturned"] = queryResult?.Count().ToString() ?? "0";
            }
            
            await ProcessQueryRecords(query, queryResult);
        }
        catch (Exception ex)
        {
            if (loadDataOperation != null)
                loadDataOperation.Telemetry.Success = false;

            Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, ex,
                "An error occured fetching data from the telemetry client");
        }
    }
    
    /// <summary>
    /// A method that will be executed on the internal auto refresh interval
    /// </summary>
    public virtual async Task UpdateDataSet()
    {
        using var updateOperation = this.TelemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(UpdateDataSet)}"); 
        
        try
        {
            this.CancellationToken?.ThrowIfCancellationRequested();
         
            this.Logger?.LogTrace("Starting update of data set {DataSetName}", DataSetName);
            
            this.TelemetryClient?.TrackEvent(new EventTelemetry()
            {
                Name = "UpdateDataSet",
                Properties =
                {
                    { "DataSetName", this.DataSetName },
                    { "DataSetId", this.DataSetId },
                    { "DataSetType", this.DataSetType }
                }
            });
            
            await ExecuteQueries();
                        
            this.OnDataSetUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            if (updateOperation != null)
                updateOperation.Telemetry.Success = false;

            Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, ex,
                "An error occured during the update of the data set {DataSetName}. {Ex}", DataSetName, ex);
        }
        finally
        {
            if (this.AutoRefreshEnabled == true && (CancellationToken?.IsCancellationRequested ?? false) == false)
            {
                this.Logger?.LogTrace("Restarting refresh timer for data set {DataSetName}", DataSetName);
                this._autoRefreshTimer.Change(AutoRefreshInterval.Value, Timeout.InfiniteTimeSpan);
            }
        }
    }
   
    /// <summary>
    /// Process and persist the data returned by the query 
    /// </summary>
    /// <param name="query">The query that was executed</param>
    /// <param name="records">The result of the query</param>
    /// <typeparam name="T">The type of data contract entity</typeparam>
    /// <returns></returns>
    protected abstract Task ProcessQueryRecords<T>(DataContractQuery<T> query,
        IEnumerable<T> records) where T : Abstractions.DataContracts.Models.DataContract; 
    
    private async void OnTimerTick(object? state)
    {
        try
        {
            await UpdateDataSet();
        }
        catch (Exception ex)
        {
            Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, ex,
                "An error occurred with the dataset update");
        }
    }
    #endregion

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed) return;
        _disposed = true;        
        Abstractions.DependencyExtensions.Mediator?.Send(new Abstractions.CQRS.DataSets.Commands.DetachDataSetCommand(this));
        _autoRefreshTimer.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
   
}