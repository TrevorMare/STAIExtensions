import { BehaviorSubject } from "rxjs";
import { CustomMetric } from "./data-contracts";
import { View } from "./view";

export interface ViewAggregate {
    numberOfCalls: number;
    startDate: Date;
    endDate: Date
}

export interface ViewAggregateGroup {
    groupName: string;
    items: ViewAggregate[]
}

export interface CustomMetricsOverviewView extends View {
    cloudNames: Record<string, string[]>;
    minTelemetryDate: Date;
    maxTelemetryDate: Date;
    totalNumberOfItems: number;
    filteredNumberOfItems: number;
    lastCustomMetricItems: CustomMetric[];
    aggregateGroups: ViewAggregateGroup[];
}

export abstract class BaseCustomMetricsOverviewService  {
   
    abstract View$: BehaviorSubject<CustomMetricsOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>): void;
    abstract SelectedCloudFilters: string[];
}