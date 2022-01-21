import { Observable } from 'rxjs';
import { CloudNames } from './cloud-names';
import { View } from './view';


export interface TelemetryOverviewView extends View {
    cloudNames?: CloudNames,
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