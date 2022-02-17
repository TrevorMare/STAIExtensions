import { Component, OnInit } from '@angular/core';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { BehaviorSubject } from 'rxjs';
import { TelemetryOverviewView } from 'src/app/data/view.telemetry-overview';
import { AIException } from 'src/app/data/data-contracts';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TelemetryOverviewService } from 'src/app/services/service.telemetry-overview';
import { JsonObjectViewerModalComponent } from 'src/app/shared/json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';
@Component({
  selector: 'telemetryoverviewpage-exceptions-table',
  templateUrl: './exceptions-table.component.html',
  styleUrls: ['./exceptions-table.component.scss']
})
export class ExceptionsTableComponent implements OnInit {
  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null!); 
  jsonViewerData: any = null;

  public configuration: Config;
  public columns: Columns[];
  public data: AIException[] = [];

  constructor(
    private modalService: NgbModal,
    public telemetryOverviewService: TelemetryOverviewService) { 
    telemetryOverviewService.View$.subscribe(value => { 
      this.telemetryOverviewView$.next(value);
      if (!!value?.lastExceptions) {
        this.data = value.lastExceptions;
      }
    });
  }

  ngOnInit(): void {
    this.configuration = { ...DefaultConfig };
    this.configuration.searchEnabled = true;
    
    this.columns = [
      { key: 'itemId', title: 'Actions', searchEnabled: false, orderEnabled: false },
      { key: 'operationName', title: 'Operation Name' },
      { key: 'timeStamp', title: 'Timestamp' },
      { key: 'outerMessage', title: 'Message' },
    ];
  }

  public onViewJsonItemClick($event: any, index: number): void {
    var item = this.data[index];
    this.jsonViewerData = item;
    const modalRef = this.modalService.open(JsonObjectViewerModalComponent, { size: 'xl', centered: true });
    modalRef.componentInstance.data = item;
  }

}
