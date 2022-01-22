import { BehaviorSubject } from 'rxjs';
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
    exceptionsCount?: number
}

export abstract class TelemetryOverviewService {
   
    abstract View$: BehaviorSubject<TelemetryOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>);
    abstract SelectedCloudFilters: string[];
}