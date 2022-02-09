import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { BehaviorSubject } from 'rxjs';
import { PerformanceCounter } from 'src/app/data/data-contracts';
import { TelemetryOverviewView } from 'src/app/data/view.telemetry-overview';
import { TelemetryOverviewService } from 'src/app/services/service.telemetry-overview';
import { JsonObjectViewerModalComponent } from 'src/app/shared/json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';

@Component({
  selector: 'st-performance-counters-table',
  templateUrl: './performance-counters-table.component.html',
  styleUrls: ['./performance-counters-table.component.scss']
})
export class PerformanceCountersTableComponent implements OnInit {

  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null!); 
  jsonViewerData: any = null;

  public configuration: Config;
  public columns: Columns[];
  public data: PerformanceCounter[] = [];

  constructor(
    private modalService: NgbModal,
    public telemetryOverviewService: TelemetryOverviewService) { 
    telemetryOverviewService.View$.subscribe(value => { 
      this.telemetryOverviewView$.next(value);
      if (!!value?.lastPerformanceCounters) {
        this.data = value.lastPerformanceCounters;
      }
    });
  }

  ngOnInit(): void {
    this.configuration = { ...DefaultConfig };
    this.configuration.searchEnabled = true;
    
    this.columns = [
      { key: 'itemId', title: 'Actions', searchEnabled: false, orderEnabled: false },
      { key: 'name', title: 'Name' },
      { key: 'timeStamp', title: 'Timestamp' },
      { key: 'category', title: 'Category' },
      { key: 'instance', title: 'Instance' },
      { key: 'value', title: 'Value' },
      { key: 'cloudRoleInstance', title: 'Role Instance' },
      { key: 'cloudRoleName', title: 'Role Name' },
      
    ];
  }

  public onViewJsonItemClick($event: any, index: number): void {
    var item = this.data[index];
    this.jsonViewerData = item;
    const modalRef = this.modalService.open(JsonObjectViewerModalComponent, { size: 'xl', centered: true });
    modalRef.componentInstance.data = item;
  }

}
