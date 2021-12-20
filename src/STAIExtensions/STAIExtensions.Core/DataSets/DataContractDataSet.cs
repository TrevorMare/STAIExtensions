using System.Collections;
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
    private const bool ExecuteTasksAsync = true;
    
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
        var loadOptions = ExtractQueryInformation<IAvailability>(_options.Availiblity, Availability);
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
        var loadOptions = ExtractQueryInformation<IBrowserTiming>(_options.BrowserTiming, BrowserTiming);
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
        var loadOptions = ExtractQueryInformation<ICustomEvent>(_options.CustomEvents, CustomEvents);
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
        var loadOptions = ExtractQueryInformation<ICustomMetrics>(_options.CustomMetrics, CustomMetrics);
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
        var loadOptions = ExtractQueryInformation<IDependency>(_options.Dependencies, Dependencies);
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
        var loadOptions = ExtractQueryInformation<IException>(_options.Exceptions, Exceptions);
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
        var loadOptions = ExtractQueryInformation<IPageView>(_options.PageViews, PageViews);
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
        var loadOptions = ExtractQueryInformation<IPerformanceCounter>(_options.PerformanceCounters, PerformanceCounters);
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
        var loadOptions = ExtractQueryInformation<IRequest>(_options.Requests, Requests);
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
        var loadOptions = ExtractQueryInformation<ITrace>(_options.Traces, Traces);
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
    
    private (bool ExecuteQuery, bool FullPeriod, DateTime? FromPeriod, int? MaximumRows) ExtractQueryInformation<T>(LoadOptions options, SizedList<T> list) where T : IDataContract
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
                fromPeriod = list.Max(x => x.TimeStamp);
            }
        }

        return new(executeQuery, fullPeriod, fromPeriod, maxRows);
    }

    protected override Task ProcessQueryRecords<T>(DataContractQuery<T> query, IEnumerable<T> records)
    {
        if (query.ContractType == typeof(IAvailability))
        {
            if (records is IEnumerable<IAvailability> items) 
                Availability.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(IBrowserTiming))
        {
            if (records is IEnumerable<IBrowserTiming> items) 
                BrowserTiming.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(ICustomEvent))
        {
            if (records is IEnumerable<ICustomEvent> items) 
                CustomEvents.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(ICustomMetrics))
        {
            if (records is IEnumerable<ICustomMetrics> items) 
                CustomMetrics.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(IDependency))
        {
            if (records is IEnumerable<IDependency> items) 
                Dependencies.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(IException))
        {
            if (records is IEnumerable<IException> items) 
                Exceptions.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(IPageView))
        {
            if (records is IEnumerable<IPageView> items) 
                PageViews.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(IPerformanceCounter))
        {
            if (records is IEnumerable<IPerformanceCounter> items) 
                PerformanceCounters.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(IRequest))
        {
            if (records is IEnumerable<IRequest> items) 
                Requests.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        else if (query.ContractType == typeof(ITrace))
        {
            if (records is IEnumerable<ITrace> items) 
                Traces.AddRange(items.OrderBy(x => x.TimeStamp));
        }
        return Task.CompletedTask;
    }
    #endregion

   
}