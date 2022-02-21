import { BehaviorSubject } from "rxjs";
import { CustomEvent, Trace } from "./data-contracts";
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

export interface CustomEventsOverviewView extends View {
    cloudNames: Record<string, string[]>;
    minTelemetryDate: Date;
    maxTelemetryDate: Date;
    totalNumberOfItems: number;
    filteredNumberOfItems: number;
    lastCustomEventItems: CustomEvent[];
    aggregateGroups: ViewAggregateGroup[];
}

export abstract class BaseCustomEventsOverviewService  {
   
    abstract View$: BehaviorSubject<CustomEventsOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>): void;
    abstract SelectedCloudFilters: string[];
}