import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import * as base from '../data/telemetry-overview'
import { STAIExtensionsService } from './staiextensions.service';


@Injectable({
  providedIn: 'root'
})
export class TelemetryOverviewService implements base.TelemetryOverviewService {
  public View$ = new BehaviorSubject<base.TelemetryOverviewView>(null);
  public ViewId$ = new BehaviorSubject<string>(null);
  public SelectedCloudFilters: string[]  = [ '-1' ];

  private ViewTypeName: string = "STAIExtensions.Default.Views.TelemetryOverview, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

  constructor(private stAIService: STAIExtensionsService) {
    this.stAIService.Ready$.subscribe((isReady) => {
      if (isReady === true) {
        this.CreateView();
      }
    });
    this.stAIService.ViewUpdated$.subscribe((view) => {
        this.OnServiceViewUpdated(view);
    });
  }
  
  
  public ApplyViewFilter(filterParameters?: Record<string, any>) {
    if (this.View$.value !== null) {
      this.stAIService.SetViewParameters(this.ViewId$.value, filterParameters);
    }
  }
  
  private CreateView(): void {
    if (this.ViewId$.value === null) {
      this.stAIService.CreateView$(this.ViewTypeName).then((view) => {
        this.View$.next(view);
        this.ViewId$.next(this.View$.value.id);
      }).catch((err) => {
        console.log(`An error occured ${err}`);
      });
    }
  }

  private OnServiceViewUpdated(view: any) {
    const id = view?.id;
    if (id !== null && id === this.ViewId$.value) {
      this.View$.next(view);
    }
  }

}
