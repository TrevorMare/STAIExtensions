using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Core.DataSets;

public abstract class DataSet : Abstractions.Data.IDataSet
{

    #region Properties
    public string DataSetName { get; set; }

    public event EventHandler? OnDataSetUpdated;
    
    public bool AutoRefreshEnabled { get; protected set; }

    protected CancellationToken? CancellationToken { get; private set; }
    
    protected ILogger<DataSet>? Logger { get; private set; }
    
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
    }
    #endregion

    #region Methods
    public void StartAutoRefresh(TimeSpan autoRefreshInterval, CancellationToken? cancellationToken = default)
    {
        this.AutoRefreshInterval = autoRefreshInterval;
        this.CancellationToken = cancellationToken;
        this.AutoRefreshEnabled = true;
        
        this.Logger?.LogInformation("Starting auto refresh on dataset {DataSetName} with interval {Interval}", DataSetName, AutoRefreshInterval);
        this._autoRefreshTimer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
    }

    public void StopAutoRefresh()
    {
        this.Logger?.LogInformation("Stopping auto refresh on dataset {DataSetName}", DataSetName);
        this.AutoRefreshEnabled = false;
        this._autoRefreshTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    protected abstract Task ExecuteQueries();

    protected virtual async Task ExecuteDataQuery<T>(DataContractQuery<T> query) where T : IDataContract
    {
        try
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
        
            this.Logger?.LogInformation("Executing query of type {TypeName} on telemetry loader", query.ContractType);
            
            var queryResult = await TelemetryLoader.ExecuteQueryAsync(query);
            
            this.Logger?.LogInformation("Query returned {NumberOfRows} rows", queryResult?.Count());
            
            await ProcessQueryRecords(query, queryResult);
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, "An error occured fetching data from the telemetry client");
        }
    }
    
    public virtual async Task UpdateDataSet()
    {
        try
        {
            this.CancellationToken?.ThrowIfCancellationRequested();
         
            this.Logger?.LogTrace("Starting update of data set {DataSetName}", DataSetName);
            
            await ExecuteQueries();
            
            this.OnDataSetUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, "An error occured during the update of the data set {DataSetName}. {Ex}", DataSetName, ex);
        }
        finally
        {
            if (this.AutoRefreshEnabled == true && CancellationToken?.IsCancellationRequested == false)
            {
                this.Logger?.LogTrace("Restarting refresh timer for data set {DataSetName}", DataSetName);
                this._autoRefreshTimer.Change(AutoRefreshInterval.Value, Timeout.InfiniteTimeSpan);
            }
        }
    }

    protected abstract Task ProcessQueryRecords<T>(DataContractQuery<T> query,
        IEnumerable<T> records) where T : IDataContract; 
    
    private async void OnTimerTick(object? state)
    {
        await UpdateDataSet();
    }
    #endregion
  
}