import { Component, EventEmitter, Input, NgZone, OnInit, Output } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TelemetryOverviewView } from '../../@core/data/telemetry-overview';
import 'rxjs/add/operator/debounceTime';
import { TelemetryOverviewService } from '../../@core/utils/telemetry-overview.service';
import { TelemetryType } from '../../@core/data/data-contracts';




@Component({
  selector: 'st-telemetry-overview',
  templateUrl: './telemetry-overview.component.html',
  styleUrls: ['./telemetry-overview.component.scss']
})
export class TelemetryOverviewComponent implements OnInit {

  private _selected: TelemetryType = TelemetryType.Availability;

  get selected(): TelemetryType {
    return this._selected;
  }
  @Input() set selected(value: TelemetryType) {
    if (value !== this._selected) {
      this._selected = value;
      this.selectedChanged.emit(this._selected);
    }
  }
  @Output() selectedChanged = new EventEmitter<TelemetryType>();
  

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
