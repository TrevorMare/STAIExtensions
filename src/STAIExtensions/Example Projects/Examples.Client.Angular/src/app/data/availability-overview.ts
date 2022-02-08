import { BehaviorSubject } from 'rxjs'
import { View } from './view';


export interface AvailabilityAggregate {
    startDate: Date;
    endDate: Date;
    maxDuration: number;
    minDuration: number;
    averageDuration: number;
    successfulCount: number;
    failureCount: number;
    totalCount: number;
    successPercentage: number;
}

export interface AvailabilityAggregateGroup {
    groupName: string;
    items: AvailabilityAggregate[];
    lastItem: AvailabilityAggregate;
    children: AvailabilityAggregateGroup[];
}

export interface AvailabilityOverviewView extends View {
    cloudNames?: Record<string, string[]>;
    totalItemsCount: number;
    filteredItemsCount: number;
    minTelemetryDate?: Date;
    maxTelemetryDate?: Date;
    aggregateGroup?: AvailabilityAggregateGroup;
}


export abstract class AvailabilityOverviewService {
   
    abstract View$: BehaviorSubject<AvailabilityOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>): void;
    abstract SelectedCloudFilters: string[];
}
