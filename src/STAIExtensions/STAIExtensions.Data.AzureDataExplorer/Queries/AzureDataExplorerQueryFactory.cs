using STAIExtensions.Abstractions.Common;
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

    public DataContractQuery<IAvailability> BuildAvailabilityQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQuery<IAvailability>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IAvailability> BuildAvailabilityQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQueryWithCustomDate<IAvailability>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IAvailability> BuildAvailabilityQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQueryWithTimeSpan<IAvailability>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IAvailability> BuildAvailabilityWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Availability.DisplayName();
        return BuildCustomQueryWithInterval<IAvailability>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Dependency

    public DataContractQuery<IDependency> BuildDependencyQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQuery<IDependency>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IDependency> BuildDependencyQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQueryWithCustomDate<IDependency>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IDependency> BuildDependencyQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQueryWithTimeSpan<IDependency>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IDependency> BuildDependencyWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Dependency.DisplayName();
        return BuildCustomQueryWithInterval<IDependency>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Exception

    public DataContractQuery<IException> BuildExceptionQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQuery<IException>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IException> BuildExceptionQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQueryWithCustomDate<IException>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IException> BuildExceptionQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQueryWithTimeSpan<IException>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IException> BuildExceptionWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Exception.DisplayName();
        return BuildCustomQueryWithInterval<IException>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Request

    public DataContractQuery<IRequest> BuildRequestQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQuery<IRequest>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IRequest> BuildRequestQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQueryWithCustomDate<IRequest>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IRequest> BuildRequestQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQueryWithTimeSpan<IRequest>(tableName, tableName, agoTimespan, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IRequest> BuildRequestWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Request.DisplayName();
        return BuildCustomQueryWithInterval<IRequest>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Trace

    public DataContractQuery<ITrace> BuildTraceQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQuery<ITrace>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<ITrace> BuildTraceQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQueryWithCustomDate<ITrace>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<ITrace> BuildTraceQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQueryWithTimeSpan<ITrace>(tableName, tableName, agoTimespan, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<ITrace> BuildTraceWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.Trace.DisplayName();
        return BuildCustomQueryWithInterval<ITrace>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Browser Timing

    public DataContractQuery<IBrowserTiming> BuildBrowserTimingQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQuery<IBrowserTiming>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IBrowserTiming> BuildBrowserTimingQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQueryWithCustomDate<IBrowserTiming>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IBrowserTiming> BuildBrowserTimingQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQueryWithTimeSpan<IBrowserTiming>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IBrowserTiming> BuildBrowserTimingWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.BrowserTiming.DisplayName();
        return BuildCustomQueryWithInterval<IBrowserTiming>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Custom Event

    public DataContractQuery<ICustomEvent> BuildCustomEventQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQuery<ICustomEvent>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<ICustomEvent> BuildCustomEventQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQueryWithCustomDate<ICustomEvent>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<ICustomEvent> BuildCustomEventQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQueryWithTimeSpan<ICustomEvent>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<ICustomEvent> BuildCustomEventWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomEvent.DisplayName();
        return BuildCustomQueryWithInterval<ICustomEvent>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Custom Metric

    public DataContractQuery<ICustomMetrics> BuildCustomMetricQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQuery<ICustomMetrics>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<ICustomMetrics> BuildCustomMetricQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQueryWithCustomDate<ICustomMetrics>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<ICustomMetrics> BuildCustomMetricQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQueryWithTimeSpan<ICustomMetrics>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<ICustomMetrics> BuildCustomMetricWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.CustomMetric.DisplayName();
        return BuildCustomQueryWithInterval<ICustomMetrics>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Page Views

    public DataContractQuery<IPageView> BuildPageViewQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQuery<IPageView>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IPageView> BuildPageViewQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQueryWithCustomDate<IPageView>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IPageView> BuildPageViewQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQueryWithTimeSpan<IPageView>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IPageView> BuildPageViewWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PageViews.DisplayName();
        return BuildCustomQueryWithInterval<IPageView>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Performance Counter

    public DataContractQuery<IPerformanceCounter> BuildPerformanceCounterQuery(
        int? topRows = default, bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQuery<IPerformanceCounter>(tableName, tableName, topRows, orderByTimestampDesc);
    }

    public DataContractQuery<IPerformanceCounter> BuildPerformanceCounterQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQueryWithCustomDate<IPerformanceCounter>(tableName, tableName, dateTimeOffset, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IPerformanceCounter> BuildPerformanceCounterQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQueryWithTimeSpan<IPerformanceCounter>(tableName, tableName, agoTimespan, topRows,
            orderByTimestampDesc);
    }

    public DataContractQuery<IPerformanceCounter> BuildPerformanceCounterWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default)
    {
        var tableName = Abstractions.Common.DataContractSource.PerformanceCounter.DisplayName();
        return BuildCustomQueryWithInterval<IPerformanceCounter>(tableName, tableName, interval, agoPeriod, topRows,
            orderByTimestampDesc);
    }

    #endregion

    #region Custom Query Methods

    public DataContractQuery<T> BuildCustomQuery<T>(
        string tableName, string alias, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract
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
        bool? orderByTimestampDesc = default) where T : IDataContract
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
        bool? orderByTimestampDesc = default) where T : IDataContract
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
        bool? orderByTimestampDesc = default) where T : IDataContract
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