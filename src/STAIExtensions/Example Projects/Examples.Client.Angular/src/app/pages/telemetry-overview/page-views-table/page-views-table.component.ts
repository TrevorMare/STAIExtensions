import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { BehaviorSubject } from 'rxjs';
import { PageView } from 'src/app/data/data-contracts';
import { TelemetryOverviewView } from 'src/app/data/view.telemetry-overview';
import { TelemetryOverviewService } from 'src/app/services/service.telemetry-overview';
import { JsonObjectViewerModalComponent } from 'src/app/shared/json-object-viewer/json-object-viewer-modal/json-object-viewer-modal.component';

@Component({
  selector: 'telemetryoverviewpage-page-views-table',
  templateUrl: './page-views-table.component.html',
  styleUrls: ['./page-views-table.component.scss']
})
export class PageViewsTableComponent implements OnInit {
 
  telemetryOverviewView$ = new BehaviorSubject<TelemetryOverviewView>(null!); 
  jsonViewerData: any = null;

  public configuration: Config;
  public columns: Columns[];
  public data: PageView[] = [];

  constructor(
    private modalService: NgbModal,
    public telemetryOverviewService: TelemetryOverviewService) { 
    telemetryOverviewService.View$.subscribe(value => { 
      this.telemetryOverviewView$.next(value);
      if (!!value?.lastPageViews) {
        this.data = value.lastPageViews;
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
      { key: 'duration', title: 'Duration (ms)' },
      { key: 'clientBrowser', title: 'Browser' },
      { key: 'clientCity', title: 'City' },
      { key: 'operationName', title: 'Operation' },
      
    ];
  }

  public onViewJsonItemClick($event: any, index: number): void {
    var item = this.data[index];
    this.jsonViewerData = item;
    const modalRef = this.modalService.open(JsonObjectViewerModalComponent, { size: 'xl', centered: true });
    modalRef.componentInstance.data = item;
  }

}
