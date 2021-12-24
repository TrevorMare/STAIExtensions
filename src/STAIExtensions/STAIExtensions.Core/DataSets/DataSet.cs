using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Core.DataSets;

public abstract class DataSet : Abstractions.Data.IDataSet
{

    #region Properties

    public List<IDataSetView> Views { get; protected set; } = new();

    public string DataSetName { get; set; }

    public string DataSetId { get; set; } = Guid.NewGuid().ToString();

    public string DataSetType => this.GetType().Name;
    
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
        
        // Attach this data set to the data set collection
        Abstractions.DependencyExtensions.Mediator?.Send(new Abstractions.CQRS.DataSets.Commands.AttachDataSetCommand(this));
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

    protected virtual async Task ExecuteDataQuery<T>(DataContractQuery<T> query) where T : Abstractions.DataContracts.Models.DataContract
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

            var updateViewTasks = new List<Task>();
            foreach (var view in Views)
            {
                updateViewTasks.Add(view.OnDataSetUpdated(this)); 
            }

            Task.WaitAll(updateViewTasks.ToArray());
            
            this.OnDataSetUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            this.Logger?.LogError(ex, "An error occured during the update of the data set {DataSetName}. {Ex}", DataSetName, ex);
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
   
    protected abstract Task ProcessQueryRecords<T>(DataContractQuery<T> query,
        IEnumerable<T> records) where T : Abstractions.DataContracts.Models.DataContract; 
    
    private async void OnTimerTick(object? state)
    {
        await UpdateDataSet();
    }
    #endregion

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Abstractions.DependencyExtensions.Mediator?.Send(new Abstractions.CQRS.DataSets.Commands.DetachDataSetCommand(this));
            _autoRefreshTimer.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
   
}