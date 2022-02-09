import { Component, OnInit } from '@angular/core';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { JsonObjectViewerModalComponent } from 'src/app/shared/json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';
import { TelemetryOverviewService } from 'src/app/services/service.telemetry-overview';
import { BehaviorSubject } from 'rxjs';
import { TelemetryOverviewView } from 'src/app/data/view.telemetry-overview';
import { Availability } from 'src/app/data/data-contracts';

@Component({
  selector: 'st-availability-table',
  templateUrl: './availability-table.component.html',
  styleUrls: ['./availability-table.component.scss']
})
export class AvailabilityTableComponent implements OnInit {
  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null!); 
  jsonViewerData: any = null;

  public configuration: Config;
  public columns: Columns[];
  public data: Availability[] = [];

  constructor(
    private modalService: NgbModal,
    public telemetryOverviewService: TelemetryOverviewService) { 
    telemetryOverviewService.View$.subscribe(value => { 
      this.telemetryOverviewView$.next(value);
      if (!!value?.lastAvailability) {
        this.data = value.lastAvailability;
      }
    });
  }

  ngOnInit(): void {
    this.configuration = { ...DefaultConfig };
    this.configuration.searchEnabled = true;
    
    this.columns = [
      { key: 'itemId', title: 'Actions', searchEnabled: false, orderEnabled: false },
      { key: 'duration', title: 'Duration (ms)' },
      { key: 'name', title: 'Name' },
      { key: 'location', title: 'Location' },
      { key: 'cloudRoleInstance', title: 'Role Instance' },
      { key: 'cloudRoleName', title: 'Role Name' },
      { key: 'success', title: 'Success' },
    ];
  }

  public onViewJsonItemClick($event: any, index: number): void {
    var item = this.data[index];
    this.jsonViewerData = item;
    const modalRef = this.modalService.open(JsonObjectViewerModalComponent, { size: 'xl', centered: true });
    modalRef.componentInstance.data = item;
  }

}
