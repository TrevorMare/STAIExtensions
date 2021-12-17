using Microsoft.Extensions.Logging;
using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;
using STAIExtensions.Core.Collections;
using Exception = STAIExtensions.Abstractions.DataContracts.Models.Exception;

namespace STAIExtensions.Core.DataSets;

public class DataSetFull : Core.Collections.QueryDataSet
{
    
    #region Properties

    public SizedList<Availability> Availability { get; } = new SizedList<Availability>();

    public SizedList<Dependency> Dependencies { get; } =  new SizedList<Dependency>();

    public SizedList<Exception> Exceptions { get; } = new SizedList<Exception>();

    public SizedList<Request> Requests { get; } = new SizedList<Request>();

    public SizedList<Trace> Traces { get; } =  new SizedList<Trace>();

    public SizedList<BrowserTiming> BrowserTimings { get; } = new SizedList<BrowserTiming>();

    public SizedList<CustomEvent> CustomEvents { get; } = new SizedList<CustomEvent>();

    public SizedList<CustomMetrics> CustomMetrics { get; } = new SizedList<CustomMetrics>();

    public SizedList<PageView> PageViews { get; } = new SizedList<PageView>();

    public SizedList<PerformanceCounter> PerformanceCounters { get; } = new SizedList<PerformanceCounter>();
    #endregion

    #region ctor
    public DataSetFull(IServiceProvider serviceProvider, ILogger<DataSetFull>? logger = default) : base(serviceProvider, logger)
    {
        this.AddDataQueries(QueryBuilder.BuildDataContractQueries(AzureApiDataContractSource.All, 5, AgoPeriod.Days, null, null));
    }
    #endregion

    #region Methods
    protected override void DeserializeRows(ApiClientQueryResultTable table, IDataContractQuery query)
    {
        if (query.DataRowDeserializer == null)
            return;
        
        var deserializedRows = query.DataRowDeserializer.Invoke(TableRowDeserializer, table);
        if (deserializedRows == null || !deserializedRows.Any())
            return;
        
        if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.Availability.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<Availability> ?? new List<Availability>();
            this.Availability.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.BrowserTiming.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<BrowserTiming> ?? new List<BrowserTiming>();
            this.BrowserTimings.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.Dependency.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<Dependency> ?? new List<Dependency>();
            this.Dependencies.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.Exception.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<Exception> ?? new List<Exception>();
            this.Exceptions.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.Request.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<Request> ?? new List<Request>();
            this.Requests.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.Trace.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<Trace> ?? new List<Trace>();
            this.Traces.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.CustomEvent.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<CustomEvent> ?? new List<CustomEvent>();
            this.CustomEvents.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.CustomMetric.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<CustomMetrics> ?? new List<CustomMetrics>();
            this.CustomMetrics.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.PageViews.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<PageView> ?? new List<PageView>();
            this.PageViews.AddRange(addItems);
        }
        else if (string.Equals(query.DeserializedTableName, AzureApiDataContractSource.PerformanceCounter.DisplayName(), StringComparison.CurrentCultureIgnoreCase))
        {
            var addItems = deserializedRows as IEnumerable<PerformanceCounter> ?? new List<PerformanceCounter>();
            this.PerformanceCounters.AddRange(addItems);
        }
    }
    #endregion
    
}