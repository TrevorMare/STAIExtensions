using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.Queries;

/// <summary>
/// Interface that defines a factory to build the various default queries with.
/// </summary>
public interface IDataContractQueryFactory
{
    
    DataContractQuery<Availability> BuildAvailabilityQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<Availability> BuildAvailabilityQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Availability> BuildAvailabilityQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Availability> BuildAvailabilityWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Dependency> BuildDependencyQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<Dependency> BuildDependencyQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Dependency> BuildDependencyQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Dependency> BuildDependencyWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<AIException> BuildExceptionQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<AIException> BuildExceptionQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<AIException> BuildExceptionQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<AIException> BuildExceptionWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Request> BuildRequestQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<Request> BuildRequestQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Request> BuildRequestQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Request> BuildRequestWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Trace> BuildTraceQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<Trace> BuildTraceQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Trace> BuildTraceQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<Trace> BuildTraceWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<BrowserTiming> BuildBrowserTimingQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<BrowserTiming> BuildBrowserTimingQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<BrowserTiming> BuildBrowserTimingQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<BrowserTiming> BuildBrowserTimingWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<CustomEvent> BuildCustomEventQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<CustomEvent> BuildCustomEventQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<CustomEvent> BuildCustomEventQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<CustomEvent> BuildCustomEventWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<CustomMetric> BuildCustomMetricQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<CustomMetric> BuildCustomMetricQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<CustomMetric> BuildCustomMetricQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<CustomMetric> BuildCustomMetricWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<PageView> BuildPageViewQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<PageView> BuildPageViewQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<PageView> BuildPageViewQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<PageView> BuildPageViewWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<PerformanceCounter> BuildPerformanceCounterQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<PerformanceCounter> BuildPerformanceCounterQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<PerformanceCounter> BuildPerformanceCounterQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<PerformanceCounter> BuildPerformanceCounterWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<T> BuildCustomQuery<T>(
        string tableName, string alias, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : DataContract;

    DataContractQuery<T> BuildCustomQueryWithCustomDate<T>(
        string tableName, string alias, DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : DataContract;

    DataContractQuery<T> BuildCustomQueryWithTimeSpan<T>(
        string tableName, string alias, TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : DataContract;

    DataContractQuery<T> BuildCustomQueryWithInterval<T>(
        string tableName, string alias, int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : DataContract;

}