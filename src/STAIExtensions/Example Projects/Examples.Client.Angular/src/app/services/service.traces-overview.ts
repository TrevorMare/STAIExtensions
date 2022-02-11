import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BaseTracesOverviewService, TracesOverviewView } from '../data/view.traces-overview';

import { STAIExtensionsService } from './staiextensions-data-service';

@Injectable({
  providedIn: 'root'
})
export class TracesOverviewService implements BaseTracesOverviewService {
  public View$ = new BehaviorSubject<TracesOverviewView>(null!);
  public ViewId$ = new BehaviorSubject<string>(null!);
  public SelectedCloudFilters: string[]  = [ '-1' ];

  private ViewTypeName: string = "TracesView";

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
        // Once the view is created, get the view
        this.stAIService.GetView$(view.id).then((getViewResponse) => {
          this.View$.next(getViewResponse  as TracesOverviewView);
          this.ViewId$.next(this.View$.value.id);

        }).catch((err) => {
          console.log(`An error occured ${err}`);
        });
      
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
