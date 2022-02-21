using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;

namespace STAIExtensions.Default.Views;

public class TelemetryOverview : BaseView
{

    #region Constants
    private const string PARAM_LASTRECORDCOUNT = "LastRecordCount"; 
    #endregion

    #region Members
    private int _lastRecordCount = 10; 

    private List<Availability> _availabilityFiltered;
    private List<BrowserTiming> _browserTimingsFiltered;
    private List<CustomEvent> _customEventsFiltered;
    private List<CustomMetric> _customMetricsFiltered;
    private List<Dependency> _dependenciesFiltered;
    private List<AIException> _exceptionsFiltered;
    private List<PageView> _pageViewsFiltered;
    private List<PerformanceCounter> _performanceCountersFiltered;
    private List<Request> _requestsFiltered;
    private List<Trace> _tracesFiltered;
    #endregion
    
    #region Properties
    public int? AvailabilityCount { get; private set; }
    
    public int? BrowserTimingsCount { get; private set; }
    
    public int? CustomEventsCount { get; private set; }
    
    public int? CustomMetricsCount { get; private set; }
    
    public int? DependenciesCount { get; private set; }
    
    public int? ExceptionsCount { get; private set; }
    
    public int? PageViewsCount { get; private set; }
    
    public int? PerformanceCountersCount { get; private set; }
    
    public int? RequestsCount { get; private set; }
    
    public int? TracesCount { get; private set; }
    
    public IEnumerable<Availability>? LastAvailability { get; private set; }
    
    public IEnumerable<BrowserTiming>? LastBrowserTimings { get; private set; }
    
    public IEnumerable<CustomEvent>? LastCustomEvents { get; private set; }
    
    public IEnumerable<CustomMetric>? LastCustomMetrics { get; private set; }
    
    public IEnumerable<Dependency>? LastDependencies { get; private set; }
    
    public IEnumerable<AIException>? LastExceptions { get; private set; }
    
    public IEnumerable<PageView>? LastPageViews { get; private set; }
    
    public IEnumerable<PerformanceCounter>? LastPerformanceCounters { get; private set; }
    
    public IEnumerable<Request>? LastRequests { get; private set; }
    
    public IEnumerable<Trace>? LastTraces { get; private set; }
    #endregion

    #region ctor

    public TelemetryOverview()
    {
        RegisterDataSetTypeForUpdates<DataContractDataSet>(set =>
        {
            BuildViewDataFromDataSet(set);
        });
    }

    #endregion
    
    #region Overrides

    public override IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors =>
        new List<DataSetViewParameterDescriptor>()
        {
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLEINSTANCE, "string[]", false, "Filter values for the cloud role instance"),
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLENAME, "string[]", false, "Filter values for the cloud role name"),
            new DataSetViewParameterDescriptor(PARAM_LASTRECORDCOUNT, "int", false, "Number of records to load for the last entities")
        };
    #endregion

    #region Build View Methods
    protected virtual Task BuildViewDataFromDataSet(DataContractDataSet dataSet)
    {
        try
        {
            this.SetupParameters();

            this.SetupDistinctCloudInstanceAndRoleNames(new List<IEnumerable<DataContractFull>>()
            {
                dataSet.Availability, dataSet.BrowserTiming, dataSet.BrowserTiming, dataSet.CustomEvents,
                dataSet.CustomMetrics, dataSet.Dependencies, dataSet.Exceptions, dataSet.PageViews,
                dataSet.PerformanceCounters, dataSet.Requests
            });
                
            this.BuildLocalListItems(dataSet);
            
            this.LoadTotals();

            this.LoadLastEntries();
        }
        catch (Exception e)
        {
            Abstractions.Common.ErrorLoggingFactory.LogError(this.TelemetryClient, this.Logger, e,
                "An error occured updating the view: {ErrorMessage}", e.Message);
        }
        
        return Task.CompletedTask;
    }

    

    #endregion

    #region Private Medhods

    /// <summary>
    /// Sets up the parameters for the view
    /// </summary>
    private void SetupParameters()
    {
        try
        {
            this.SetupCloudRoleAndInstanceNamesFromParameters();

            var lastRecordCount =
                Helpers.ViewParameterHelper.ExtractParameter<int?>(this.ViewParameters, PARAM_LASTRECORDCOUNT);
            
            if (lastRecordCount.HasValue && lastRecordCount >= 0)
            {
                this._lastRecordCount = lastRecordCount.Value;
            }
        }
        catch (Exception ex)
        {
            Abstractions.Common.ErrorLoggingFactory.LogError(base.TelemetryClient, base.Logger, ex,
                "An error occured deserializing the view parameters: {ErrorMessage}", ex.Message);
        }
    }

    /// <summary>
    /// Filters the records by the view filters
    /// </summary>
    private void BuildLocalListItems(DataContractDataSet dataContractDataSet)
    {
        // Keep a copy of the lists before it's filtered 
        this._availabilityFiltered = dataContractDataSet.Availability
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._browserTimingsFiltered = dataContractDataSet.BrowserTiming
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._customEventsFiltered = dataContractDataSet.CustomEvents
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._customMetricsFiltered = dataContractDataSet.CustomMetrics
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._dependenciesFiltered = dataContractDataSet.Dependencies
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._pageViewsFiltered = dataContractDataSet.PageViews
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._performanceCountersFiltered = dataContractDataSet.PerformanceCounters
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._requestsFiltered = dataContractDataSet.Requests
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._tracesFiltered = dataContractDataSet.Traces
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
        this._exceptionsFiltered = dataContractDataSet.Exceptions
            .FilterCloudRoleInstance(FilterCloudInstanceNames)
            .FilterCloudRoleName(FilterCloudRoleNames).ToList();
    }

    /// <summary>
    /// Load the Totals of the items
    /// </summary>
    private void LoadTotals()
    {
        this.AvailabilityCount = _availabilityFiltered.Count;
        this.BrowserTimingsCount = _browserTimingsFiltered.Count;
        this.CustomEventsCount = _customEventsFiltered.Count;
        this.CustomMetricsCount = _customMetricsFiltered.Count;
        this.DependenciesCount = _dependenciesFiltered.Count;
        this.PageViewsCount = _pageViewsFiltered.Count;
        this.PerformanceCountersCount = _performanceCountersFiltered.Count;
        this.RequestsCount = _requestsFiltered.Count;
        this.TracesCount = _tracesFiltered.Count;
        this.ExceptionsCount = _exceptionsFiltered.Count;
    }
    
    private void LoadLastEntries()
    {
        this.LastAvailability = 
           (_lastRecordCount == 0) ? new List<Availability>() : _availabilityFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastBrowserTimings = 
            (_lastRecordCount == 0) ? new List<BrowserTiming>() : _browserTimingsFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastCustomEvents = 
            (_lastRecordCount == 0) ? new List<CustomEvent>() : _customEventsFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastCustomMetrics = 
            (_lastRecordCount == 0) ? new List<CustomMetric>() : _customMetricsFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastDependencies = 
            (_lastRecordCount == 0) ? new List<Dependency>() : _dependenciesFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastExceptions = 
            (_lastRecordCount == 0) ? new List<AIException>() : _exceptionsFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastPageViews = 
            (_lastRecordCount == 0) ? new List<PageView>() : _pageViewsFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastPerformanceCounters = 
            (_lastRecordCount == 0) ? new List<PerformanceCounter>() : _performanceCountersFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastRequests = 
            (_lastRecordCount == 0) ? new List<Request>() : _requestsFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
        this.LastTraces = 
            (_lastRecordCount == 0) ? new List<Trace>() : _tracesFiltered.OrderByDescending(r => r.TimeStamp).Take(_lastRecordCount);
    }
    #endregion
    
}