using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Core.Collections;
using STAIExtensions.Core.DataSets;
using STAIExtensions.Default.DataSets.Options;

namespace STAIExtensions.Default.DataSets;

public class DataContractDataSet : DataSet
{

    #region Members

    private DataContractDataSetOptions _options;
    private const bool ExecuteTasksAsync = true;
    
    #endregion

    #region Properties

    public SizedList<Availability> Availability { get; private set; } = new();

    public SizedList<BrowserTiming> BrowserTiming { get; private set; } = new();
    
    public SizedList<CustomEvent> CustomEvents { get; private set; } = new();

    public SizedList<CustomMetric> CustomMetrics { get; private set; } = new();
    
    public SizedList<Dependency> Dependencies { get; private set; } = new();
    
    public SizedList<AIException> Exceptions { get; private set; } = new();
    
    public SizedList<PageView> PageViews { get; private set; } = new();
    
    public SizedList<PerformanceCounter> PerformanceCounters { get; private set; } = new();
    
    public SizedList<Request> Requests { get; private set; } = new();
    
    public SizedList<Trace> Traces { get; private set; } = new();
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
    protected override async Task ExecuteQueries()
    {
        if (TelemetryLoader.DataContractQueryFactory == null)
            return;

        if (ExecuteTasksAsync)
        {
            var queryTasks = new List<Task>
            {
                ExecuteAvailabilityQuery(),
                ExecuteBrowserTimingQuery(),
                ExecuteCustomEventsQuery(),
                ExecuteCustomMetricsQuery(),
                ExecuteDependenciesQuery(),
                ExecuteExceptionsQuery(),
                ExecutePageViewsQuery(),
                ExecutePerformanceCountersQuery(),
                ExecuteRequestsQuery(),
                ExecuteTracesQuery()
            };

            Task.WaitAll(queryTasks.ToArray());
        }
        else
        {
            await ExecuteAvailabilityQuery();
            await ExecuteBrowserTimingQuery();
            await ExecuteCustomEventsQuery();
            await ExecuteCustomMetricsQuery();
            await ExecuteDependenciesQuery();
            await ExecuteExceptionsQuery();
            await ExecutePageViewsQuery();
            await ExecutePerformanceCountersQuery();
            await ExecuteRequestsQuery();
            await ExecuteTracesQuery();
        }
    }
    
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
    
    private async Task ExecuteAvailabilityQuery()
    {
        
        var loadOptions = ExtractQueryInformation<Availability>(_options.Availiblity, Availability);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildAvailabilityQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildAvailabilityQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecuteBrowserTimingQuery()
    {
        var loadOptions = ExtractQueryInformation<BrowserTiming>(_options.BrowserTiming, BrowserTiming);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildBrowserTimingQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildBrowserTimingQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecuteCustomEventsQuery()
    {
        var loadOptions = ExtractQueryInformation<CustomEvent>(_options.CustomEvents, CustomEvents);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildCustomEventQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildCustomEventQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecuteCustomMetricsQuery()
    {
        var loadOptions = ExtractQueryInformation<CustomMetric>(_options.CustomMetrics, CustomMetrics);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildCustomMetricQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildCustomMetricQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecuteDependenciesQuery()
    {
        var loadOptions = ExtractQueryInformation<Dependency>(_options.Dependencies, Dependencies);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildDependencyQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildDependencyQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecuteExceptionsQuery()
    {
        var loadOptions = ExtractQueryInformation<AIException>(_options.Exceptions, Exceptions);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildExceptionQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildExceptionQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecutePageViewsQuery()
    {
        var loadOptions = ExtractQueryInformation<PageView>(_options.PageViews, PageViews);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildPageViewQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildPageViewQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }

    private async Task ExecutePerformanceCountersQuery()
    {
        var loadOptions = ExtractQueryInformation<PerformanceCounter>(_options.PerformanceCounters, PerformanceCounters);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildPerformanceCounterQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildPerformanceCounterQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }

    private async Task ExecuteRequestsQuery()
    {
        var loadOptions = ExtractQueryInformation<Request>(_options.Requests, Requests);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildRequestQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildRequestQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private async Task ExecuteTracesQuery()
    {
        var loadOptions = ExtractQueryInformation<Trace>(_options.Traces, Traces);
        if (loadOptions.ExecuteQuery)
        {
            if (loadOptions.FullPeriod == true)
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildTraceQuery(loadOptions.MaximumRows, true));
            else
            {
                await ExecuteDataQuery(
                    TelemetryLoader.DataContractQueryFactory.BuildTraceQueryWithCustomDate(loadOptions.FromPeriod.Value, loadOptions.MaximumRows, true));
            }
        } 
    }
    
    private (bool ExecuteQuery, bool FullPeriod, DateTime? FromPeriod, int? MaximumRows) ExtractQueryInformation<T>(LoadOptions options, SizedList<T> list) where T : DataContract
    {
        var executeQuery = false;
        var fullPeriod = true;
        DateTime? fromPeriod = null;
        int? maxRows = null;
        
        if (options.Enabled == true)
        {
            maxRows = options.TelemetryLoadMaxRows;
            executeQuery = true;
            if (list.Any())
            {
                fullPeriod = false;
                fromPeriod = DateTime.Now;
            }
        }

        return new(executeQuery, fullPeriod, fromPeriod, maxRows);
    }

    protected override Task ProcessQueryRecords<T>(DataContractQuery<T> query, IEnumerable<T> records)
    {
        if (query.ContractType == typeof(Availability))
        {
            if (records is IEnumerable<Availability> items) 
                Availability.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(BrowserTiming))
        {
            if (records is IEnumerable<BrowserTiming> items) 
                BrowserTiming.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(CustomEvent))
        {
            if (records is IEnumerable<CustomEvent> items) 
                CustomEvents.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(CustomMetric))
        {
            if (records is IEnumerable<CustomMetric> items) 
                CustomMetrics.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(Dependency))
        {
            if (records is IEnumerable<Dependency> items) 
                Dependencies.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(AIException))
        {
            if (records is IEnumerable<AIException> items) 
                Exceptions.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(PageView))
        {
            if (records is IEnumerable<PageView> items) 
                PageViews.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(PerformanceCounter))
        {
            if (records is IEnumerable<PerformanceCounter> items) 
                PerformanceCounters.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(Request))
        {
            if (records is IEnumerable<Request> items) 
                Requests.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(Trace))
        {
            if (records is IEnumerable<Trace> items) 
                Traces.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        return Task.CompletedTask;
    }
    #endregion

   
}