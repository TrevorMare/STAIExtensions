import { Observable } from 'rxjs';
import { CloudNames } from './cloud-names';
import { View } from './view';


export interface TelemetryOverviewView extends View {
    CloudNames?: CloudNames[],
    AvailabilityCount?: number,
    BrowserTimingsCount?: number,
    CustomEventsCount?: number,
    CustomMetricsCount?: number,
    DependenciesCount?: number,
    PageViewsCount?: number,
    PerformanceCountersCount?: number,
    RequestsCount?: number,
    TracesCount?: number
}