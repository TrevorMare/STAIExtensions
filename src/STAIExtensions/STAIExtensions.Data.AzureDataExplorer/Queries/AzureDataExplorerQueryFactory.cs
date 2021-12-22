using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Data.AzureDataExplorer.Queries;

public class AzureDataExplorerQueryFactory : IDataContractQueryFactory
{

    #region Extra Methods

    public string GetDataContractSourceTableName(Abstractions.Common.DataContractSource source)
    {
        return source.DisplayName();
    }

    #endregion

    #region Availibility

    public DataContractQuery<Availability> BuildAvailabilityQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQuery<Availability>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<Availability> BuildAvailabilityQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQueryWithCustomDate<Availability>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<Availability> BuildAvailabilityQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQueryWithTimeSpan<Availability>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<Availability> BuildAvailabilityWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQueryWithInterval<Availability>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Dependency

    public DataContractQuery<Dependency> BuildDependencyQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQuery<Dependency>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<Dependency> BuildDependencyQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQueryWithCustomDate<Dependency>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<Dependency> BuildDependencyQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQueryWithTimeSpan<Dependency>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<Dependency> BuildDependencyWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQueryWithInterval<Dependency>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Exception

    public DataContractQuery<AIException> BuildExceptionQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQuery<AIException>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<AIException> BuildExceptionQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQueryWithCustomDate<AIException>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<AIException> BuildExceptionQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQueryWithTimeSpan<AIException>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<AIException> BuildExceptionWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQueryWithInterval<AIException>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Request

    public DataContractQuery<Request> BuildRequestQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQuery<Request>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<Request> BuildRequestQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQueryWithCustomDate<Request>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<Request> BuildRequestQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQueryWithTimeSpan<Request>(tableName, tableName, agoTimespan, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<Request> BuildRequestWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQueryWithInterval<Request>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Trace

    public DataContractQuery<Trace> BuildTraceQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQuery<Trace>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<Trace> BuildTraceQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQueryWithCustomDate<Trace>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<Trace> BuildTraceQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQueryWithTimeSpan<Trace>(tableName, tableName, agoTimespan, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<Trace> BuildTraceWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQueryWithInterval<Trace>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Browser Timing

    public DataContractQuery<BrowserTiming> BuildBrowserTimingQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQuery<BrowserTiming>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<BrowserTiming> BuildBrowserTimingQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQueryWithCustomDate<BrowserTiming>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<BrowserTiming> BuildBrowserTimingQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQueryWithTimeSpan<BrowserTiming>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<BrowserTiming> BuildBrowserTimingWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQueryWithInterval<BrowserTiming>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Custom Event

    public DataContractQuery<CustomEvent> BuildCustomEventQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQuery<CustomEvent>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<CustomEvent> BuildCustomEventQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQueryWithCustomDate<CustomEvent>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<CustomEvent> BuildCustomEventQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQueryWithTimeSpan<CustomEvent>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<CustomEvent> BuildCustomEventWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQueryWithInterval<CustomEvent>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Custom Metric

    public DataContractQuery<CustomMetric> BuildCustomMetricQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQuery<CustomMetric>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<CustomMetric> BuildCustomMetricQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQueryWithCustomDate<CustomMetric>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<CustomMetric> BuildCustomMetricQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQueryWithTimeSpan<CustomMetric>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<CustomMetric> BuildCustomMetricWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQueryWithInterval<CustomMetric>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Page Views

    public DataContractQuery<PageView> BuildPageViewQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQuery<PageView>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<PageView> BuildPageViewQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQueryWithCustomDate<PageView>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<PageView> BuildPageViewQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQueryWithTimeSpan<PageView>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<PageView> BuildPageViewWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQueryWithInterval<PageView>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Performance Counter

    
    
    public DataContractQuery<PerformanceCounter> BuildPerformanceCounterQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQuery<PerformanceCounter>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<PerformanceCounter> BuildPerformanceCounterQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQueryWithCustomDate<PerformanceCounter>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<PerformanceCounter> BuildPerformanceCounterQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQueryWithTimeSpan<PerformanceCounter>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<PerformanceCounter> BuildPerformanceCounterWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQueryWithInterval<PerformanceCounter>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Custom Query Methods

    public DataContractQuery<T> BuildCustomQuery<T>(
        string tableName, string alias, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : Abstractions.DataContracts.Models.DataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };
        return result;
    }

    public DataContractQuery<T> BuildCustomQueryWithCustomDate<T>(
        string tableName, string alias, DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : Abstractions.DataContracts.Models.DataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        parameterData.SetupQueryForCustom(dateTimeOffset);

        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };

        return result;
    }

    public DataContractQuery<T> BuildCustomQueryWithTimeSpan<T>(
        string tableName, string alias, TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : Abstractions.DataContracts.Models.DataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        parameterData.SetupQueryForTimespan(agoTimespan);

        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };

        return result;
    }

    public DataContractQuery<T> BuildCustomQueryWithInterval<T>(
        string tableName, string alias, int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : Abstractions.DataContracts.Models.DataContract
    {
        var parameterData = new AzureDataExplorerQueryParameter(tableName, alias, orderByTimestampDesc, topRows);
        parameterData.SetupQueryForInterval(interval, agoPeriod);

        var result = new AzureDataExplorerQuery<T>()
        {
            QueryParameterData = parameterData
        };

        return result;
    }

    #endregion

}