import { Component, NgZone, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TelemetryOverviewView } from '../../@core/data/telemetry-overview';
import 'rxjs/add/operator/debounceTime';
import { TelemetryOverviewService } from '../../@core/utils/telemetry-overview.service';

@Component({
  selector: 'st-telemetry-overview',
  templateUrl: './telemetry-overview.component.html',
  styleUrls: ['./telemetry-overview.component.scss']
})
export class TelemetryOverviewComponent implements OnInit {

  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null); 

  constructor(private zone: NgZone,
              public telemetryOverviewService: TelemetryOverviewService) { 
    
    telemetryOverviewService.View$.subscribe(value => { 
      this.zone.run(() => {
        this.telemetryOverviewView$.next(value)
      })
    });
  }

  ngOnInit(): void { 
  }

  ngOnDestroy(): void {
  }

}
