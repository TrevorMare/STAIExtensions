using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Core.Collections;
using STAIExtensions.Core.DataSets.Options;

namespace STAIExtensions.Core.DataSets;

public class DataContractDataSet : DataSet
{

    #region Members

    private DataContractDataSetOptions _options;

    #endregion

    #region Properties

    public SizedList<IAvailability> Availability { get; private set; } = new SizedList<IAvailability>();
    
    public SizedList<IBrowserTiming> BrowserTiming { get; private set; } = new SizedList<IBrowserTiming>();
    
    public SizedList<ICustomEvent> CustomEvents { get; private set; } = new SizedList<ICustomEvent>();

    public SizedList<ICustomMetrics> CustomMetrics { get; private set; } = new SizedList<ICustomMetrics>();
    
    public SizedList<IDependency> Dependencies { get; private set; } = new SizedList<IDependency>();
    
    public SizedList<IException> Exceptions { get; private set; } = new SizedList<IException>();
    
    public SizedList<IPageView> PageViews { get; private set; } = new SizedList<IPageView>();
    
    public SizedList<IPerformanceCounter> PerformanceCounters { get; private set; } = new SizedList<IPerformanceCounter>();
    
    public SizedList<IRequest> Requests { get; private set; } = new SizedList<IRequest>();
    
    public SizedList<ITrace> Traces { get; private set; } = new SizedList<ITrace>();
    #endregion
    
    #region ctor
    public DataContractDataSet(ITelemetryLoader telemetryLoader, DataContractDataSetOptions options, string? dataSetName = default) 
        : base(telemetryLoader, dataSetName)
    {
        _options = options ??= new DataContractDataSetOptions();
        
        SetInternalBufferSizes(_options);
    }
    #endregion

    #region Methods
    private void SetInternalBufferSizes(DataContractDataSetOptions options)
    {
        this.Availability.MaxSize = options.Availiblity.BufferSize;
        this.BrowserTiming.MaxSize = options.BrowserTiming.BufferSize;
        this.CustomEvents.MaxSize = options.CustomEvents.BufferSize;
        this.CustomMetrics.MaxSize = options.CustomMetrics.BufferSize;
        this.Dependencies.MaxSize = options.Dependencies.BufferSize;
        this.Exceptions.MaxSize = options.Exceptions.BufferSize;
        this.PageViews.MaxSize = options.PageViews.BufferSize;
        this.PerformanceCounters.MaxSize = options.PerformanceCounters.BufferSize;
        this.Requests.MaxSize = options.Requests.BufferSize;
        this.Traces.MaxSize = options.Traces.BufferSize;
    }
    #endregion

    public override IEnumerable<DataContractQuery<IDataContract>> GetQueriesToExecute()
    {
        if (TelemetryLoader.DataContractQueryFactory == null)
            return new List<DataContractQuery<IDataContract>>();
        
        if (_options.Availiblity.Enabled)
        {
            var x = TelemetryLoader.DataContractQueryFactory.BuildAvailabilityQuery();
            //collection.AddItem(x);
        }

        return new List<DataContractQuery<IDataContract>>();
       
    }
    
 

    public override Task ProcessQueryRecords(IDataContractQuery<IDataContract> query, IEnumerable<IDataContract> records)
    {
        if (query.ContractType is IAvailability)
        {
            if (records is IEnumerable<IAvailability> items)
                Availability.AddRange(items);
        }
        
        
        
        return Task.CompletedTask;
    }
}