import { BehaviorSubject } from 'rxjs';
import { AIException, Availability, BrowserTiming, CustomEvent, CustomMetric, Dependency, PageView, PerformanceCounter, Request, Trace } from './data-contracts';
import { View } from './view';

export interface TelemetryOverviewView extends View {
    cloudNames?: Record<string, string[]>,
    availabilityCount?: number,
    browserTimingsCount?: number,
    customEventsCount?: number,
    customMetricsCount?: number,
    dependenciesCount?: number,
    pageViewsCount?: number,
    performanceCountersCount?: number,
    requestsCount?: number,
    tracesCount?: number,
    exceptionsCount?: number,
    lastAvailability?: Availability[],
    lastBrowserTimings?: BrowserTiming[],
    lastCustomEvents?: CustomEvent[],
    lastCustomMetrics?: CustomMetric[],
    lastDependencies?: Dependency[],
    lastExceptions?: AIException[],
    lastPageViews?: PageView[],
    lastPerformanceCounters?: PerformanceCounter[],
    lastRequests?: Request[],
    lastTraces?: Trace[],
}

export abstract class TelemetryOverviewService {
   
    abstract View$: BehaviorSubject<TelemetryOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>);
    abstract SelectedCloudFilters: string[];
}