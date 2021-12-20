using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Core.DataSets;

public abstract class DataSet : Abstractions.Data.IDataSet
{

    #region Properties
    public string DataSetName { get; set; }

    public event EventHandler OnDataSetUpdated;
    
    public bool AutoRefreshEnabled { get; set; }

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
        
        this._autoRefreshTimer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
    }

    public void StopAutoRefresh()
    {
        this.AutoRefreshEnabled = false;
        this._autoRefreshTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    public abstract IEnumerable<IDataContractQuery<IDataContract>> GetQueriesToExecute();

    public virtual async Task UpdateDataSet()
    {
        try
        {
            this.CancellationToken?.ThrowIfCancellationRequested();
            var queriesToExecute = GetQueriesToExecute();

            foreach (var dataContractQuery in queriesToExecute)
            {
                if (dataContractQuery.Enabled == true)
                {
                    var dataContractQueryItems = await TelemetryLoader.ExecuteQueryAsync(dataContractQuery);
                    await ProcessQueryRecords(dataContractQuery, dataContractQueryItems);
                }
            }
            
            this.OnDataSetUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            if (this.AutoRefreshEnabled == true && CancellationToken?.IsCancellationRequested == false)
            {
                this._autoRefreshTimer.Change(AutoRefreshInterval.Value, Timeout.InfiniteTimeSpan);
            }
        }
    }

    public abstract Task ProcessQueryRecords(IDataContractQuery<IDataContract> query,
        IEnumerable<IDataContract> records); 
    
    private async void OnTimerTick(object? state)
    {
        await UpdateDataSet();
    }
    #endregion
  
}