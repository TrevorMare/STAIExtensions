using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.DataContracts;
using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Abstractions.Queries;

public interface IDataContractQueryFactory
{

    DataContractQuery<IAvailability> BuildAvailabilityQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IAvailability> BuildAvailabilityQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IAvailability> BuildAvailabilityQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IAvailability> BuildAvailabilityWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IDependency> BuildDependencyQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IDependency> BuildDependencyQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IDependency> BuildDependencyQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IDependency> BuildDependencyWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IException> BuildExceptionQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IException> BuildExceptionQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IException> BuildExceptionQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IException> BuildExceptionWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IRequest> BuildRequestQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IRequest> BuildRequestQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IRequest> BuildRequestQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IRequest> BuildRequestWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ITrace> BuildTraceQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<ITrace> BuildTraceQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ITrace> BuildTraceQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ITrace> BuildTraceWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IBrowserTiming> BuildBrowserTimingQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IBrowserTiming> BuildBrowserTimingQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IBrowserTiming> BuildBrowserTimingQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IBrowserTiming> BuildBrowserTimingWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomEvent> BuildCustomEventQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomEvent> BuildCustomEventQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomEvent> BuildCustomEventQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomEvent> BuildCustomEventWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomMetrics> BuildCustomMetricQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomMetrics> BuildCustomMetricQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomMetrics> BuildCustomMetricQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<ICustomMetrics> BuildCustomMetricWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IPageView> BuildPageViewQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IPageView> BuildPageViewQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IPageView> BuildPageViewQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IPageView> BuildPageViewWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IPerformanceCounter> BuildPerformanceCounterQuery(
        int? topRows = default, bool? orderByTimestampDesc = default);

    DataContractQuery<IPerformanceCounter> BuildPerformanceCounterQueryWithCustomDate(
        DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IPerformanceCounter> BuildPerformanceCounterQueryWithTimeSpan(
        TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<IPerformanceCounter> BuildPerformanceCounterWithInterval(
        int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default);

    DataContractQuery<T> BuildCustomQuery<T>(
        string tableName, string alias, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract;

    DataContractQuery<T> BuildCustomQueryWithCustomDate<T>(
        string tableName, string alias, DateTimeOffset dateTimeOffset, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract;

    DataContractQuery<T> BuildCustomQueryWithTimeSpan<T>(
        string tableName, string alias, TimeSpan agoTimespan, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract;

    DataContractQuery<T> BuildCustomQueryWithInterval<T>(
        string tableName, string alias, int interval, AgoPeriod agoPeriod, int? topRows = default,
        bool? orderByTimestampDesc = default) where T : IDataContract;

}