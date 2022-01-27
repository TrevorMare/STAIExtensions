using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Views;
using STAIExtensions.Default.DataSets;
using STAIExtensions.Default.Helpers;

namespace STAIExtensions.Default.Views;

public class TelemetryOverview : DataSetView
{

    #region Constants

    private const string PARAM_CLOUDROLENAME = "CloudRoleName";
    private const string PARAM_CLOUDROLEINSTANCE = "CloudRoleInstance";
    private const string PARAM_LASTRECORDCOUNT = "LastRecordCount";
    #endregion

    #region Members

    private List<string>? _filterCloudRoleName = null;
    private List<string>? _filterCloudInstanceName = null;
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

    public Dictionary<string, List<string>> CloudNames { get; private set; } = new();

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
    
    #region Overrides

    public override IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors =>
        new List<DataSetViewParameterDescriptor>()
        {
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLEINSTANCE, "string[]", false, "Filter values for the cloud role instance"),
            new DataSetViewParameterDescriptor(PARAM_CLOUDROLENAME, "string[]", false, "Filter values for the cloud role name"),
            new DataSetViewParameterDescriptor(PARAM_LASTRECORDCOUNT, "int", false, "Number of records to load for the last entities")
        };

    protected override Task BuildViewData(IDataSet dataSet)
    {
        try
        {
            if (dataSet is DataContractDataSet dataContractDataSet)
            {
                this.SetupParameters();

                this.LoadDistinctCloudNames(dataContractDataSet);
                
                this.BuildLocalListItems(dataContractDataSet);
            
                this.LoadTotals();

                this.LoadLastEntries();
            }
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
            this._filterCloudRoleName =
                Helpers.ViewParameterHelper.ExtractParameter<List<string>>(this.ViewParameters, PARAM_CLOUDROLENAME);
            this._filterCloudInstanceName =
                Helpers.ViewParameterHelper.ExtractParameter<List<string>>(this.ViewParameters, PARAM_CLOUDROLEINSTANCE);

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
    /// Loads a list of distinct Cloud Role Instance and Cloud Role Name values
    /// </summary>
    /// <param name="dataContractDataSet">The data set containing the original items</param>
    private void LoadDistinctCloudNames(DataContractDataSet dataContractDataSet)
    {
        var result = new List<string?>();
        
        result.AddRange(dataContractDataSet.Availability.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}" ).Distinct());
        result.AddRange(dataContractDataSet.BrowserTiming.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.CustomEvents.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.CustomMetrics.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.Dependencies.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.Exceptions.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.PageViews.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.PerformanceCounters.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.Requests.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());
        result.AddRange(dataContractDataSet.Traces.Select(r => $"{r.CloudRoleInstance}*{r.CloudRoleName}").Distinct());

        var distinctNames = result.Select(x => x).Distinct().ToList();
        var distinctSplitItems = distinctNames.Select(n => n?.Split("*")).Where(x => x != null).Select(n => new
        {
            Instance = n[0],
            Role = n[1]
        }).ToList();

        this.CloudNames.Clear();
        distinctSplitItems.ForEach(item =>
        {
            if (!this.CloudNames.ContainsKey(item.Instance))
                this.CloudNames[item.Instance] = new List<string>();
            this.CloudNames[item.Instance].Add(item.Role);
        });
    }

    /// <summary>
    /// Filters the records by the view filters
    /// </summary>
    private void BuildLocalListItems(DataContractDataSet dataContractDataSet)
    {
        // Keep a copy of the lists before it's filtered 
        this._availabilityFiltered = dataContractDataSet.Availability
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._browserTimingsFiltered = dataContractDataSet.BrowserTiming
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._customEventsFiltered = dataContractDataSet.CustomEvents
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._customMetricsFiltered = dataContractDataSet.CustomMetrics
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._dependenciesFiltered = dataContractDataSet.Dependencies
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._pageViewsFiltered = dataContractDataSet.PageViews
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._performanceCountersFiltered = dataContractDataSet.PerformanceCounters
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._requestsFiltered = dataContractDataSet.Requests
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._tracesFiltered = dataContractDataSet.Traces
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
        this._exceptionsFiltered = dataContractDataSet.Exceptions
            .FilterCloudRoleInstance(_filterCloudInstanceName)
            .FilterCloudRoleName(_filterCloudRoleName).ToList();
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