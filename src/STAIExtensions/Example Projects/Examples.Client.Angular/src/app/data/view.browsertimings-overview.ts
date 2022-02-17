import { BehaviorSubject } from 'rxjs';
import { BrowserTiming } from './data-contracts';
import { View } from './view';

export interface StatisticValues {
    minimum: number;
    maximum: number;
    average: number;
}

export interface GroupedStatistics {
    totalValues: StatisticValues;
    networkValues: StatisticValues;
    processingValues: StatisticValues;
    receiveValues: StatisticValues;
    sendValues: StatisticValues;
    numberOfItems: number;
    groupName: string;
    slowestOperations: string[];
    fastestOperations: string[];
}

export interface GroupValues {
    statistics: GroupedStatistics[];
}

export interface BrowserTimingsOverviewView extends View {
    cloudNames: Record<string, string[]>;
    totalNumberOfItems: number;
    filteredNumberOfItems: number;
    lastItems: BrowserTiming[];
    clientBrowserStatistics: GroupValues;
    clientCityStatistics: GroupValues;
    countryOrRegionStatistics: GroupValues;
    userSessionStatistics: GroupValues;
    userIdStatistics: GroupValues;
    operationNameStatistics: GroupValues;
}

export abstract class BaseBrowserTimingsOverviewService  {
   
    abstract View$: BehaviorSubject<BrowserTimingsOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>): void;
    abstract SelectedCloudFilters: string[];
}