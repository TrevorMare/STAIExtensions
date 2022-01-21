import { Component, NgZone, OnInit } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { TelemetryOverviewView } from '../../@core/data/telemetry-overview';
import { STAIExtensionsService } from '../../@core/utils/staiextensions.service';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'st-telemetry-overview',
  templateUrl: './telemetry-overview.component.html',
  styleUrls: ['./telemetry-overview.component.scss']
})
export class TelemetryOverviewComponent implements OnInit {

  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null);

  constructor(private zone: NgZone, 
    private staiextensionsService: STAIExtensionsService) { 

    this.staiextensionsService.Ready$.subscribe((isReady) => {
      if (isReady === true) {
        this.CreateViews();
      }
    });

    this.staiextensionsService.ViewUpdated$.subscribe((view) => {
        this.UpdateViews(view);
    });
  }

  ngOnInit(): void { 
  }

  ngOnDestroy(): void {
    if (!!this.telemetryOverviewView$?.value) {
      this.staiextensionsService.RemoveView$(this.telemetryOverviewView$.value.id);
    }
  }
 
  private UpdateViews(view: any) {
    this.zone.run(() => {
      this.telemetryOverviewView$.next(view);
    })
  }


  private CreateViews(): void {
    this.staiextensionsService.CreateView$("STAIExtensions.Default.Views.TelemetryOverview, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
    .then((view) => {
      this.telemetryOverviewView$.next(view);
    }).catch((err) => {
      console.log(`An error occured ${err}`);
    });
  }

  ApplyViewFilter(filterParameters: any): void {
    this.staiextensionsService.SetViewParameters(this.telemetryOverviewView$.value.id, filterParameters);
  }
   

}
