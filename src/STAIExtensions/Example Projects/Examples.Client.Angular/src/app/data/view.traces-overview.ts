import { BehaviorSubject } from "rxjs";
import { Trace } from "./data-contracts";
import { View } from "./view";

export interface TracesOverviewView extends View {
    cloudNames: Record<string, string[]>;
    traceItems: Trace[];
}

export abstract class BaseTracesOverviewService  {
   
    abstract View$: BehaviorSubject<TracesOverviewView>;
    abstract ViewId$ : BehaviorSubject<string>;
    abstract ApplyViewFilter(filterParameters?: Record<string, any>): void;
    abstract SelectedCloudFilters: string[];
}