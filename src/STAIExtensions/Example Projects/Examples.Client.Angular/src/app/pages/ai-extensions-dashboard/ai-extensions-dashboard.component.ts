import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { TelemetryOverviewView } from '../../@core/data/telemetry-overview';
import { STAIExtensionsService } from '../../@core/utils';

@Component({
  selector: 'ngx-ai-extensions-dashboard',
  templateUrl: './ai-extensions-dashboard.component.html',
  styleUrls: ['./ai-extensions-dashboard.component.scss']
})
export class AiExtensionsDashboardComponent implements OnInit, OnDestroy {

  public telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null);
  

  constructor(private zone: NgZone, 
              private staiextensionsService: STAIExtensionsService) { 
    
    this.staiextensionsService.Ready$.subscribe((b) => {
      if (b === true) {
        this.CreateViews();
      }
    });
    
    this.staiextensionsService.ViewUpdated$.subscribe((view) => {
      this.UpdateViews(view);
    });
  }


  ngOnDestroy(): void {
    if (!!this.telemetryOverviewView$?.value) {
      this.staiextensionsService.RemoveView(this.telemetryOverviewView$.value.id);
    }
  }

  ngOnInit(): void {
    
  }

  private UpdateViews(view: any) {
    
    this.zone.run(() => {
      this.telemetryOverviewView$.next(view);
    })
  }

  private CreateViews(): void {
    this.staiextensionsService.CreateView("STAIExtensions.Default.Views.TelemetryOverview, STAIExtensions.Default, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
    .then((view) => {
      this.telemetryOverviewView$.next(view);
    }).catch((err) => {
      console.log(`An error occured ${err}`);
    });
  }

}
