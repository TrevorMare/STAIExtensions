import { Component, NgZone, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { NbDialogService } from '@nebular/theme';
import { TelemetryOverviewService } from '../../@core/utils/telemetry-overview.service';
import { ViewSourceModalComponent } from '../view-source-modal/view-source-modal.component';
import { JsonColumnComponent } from '../json-column/json-column.component';

@Component({
  selector: 'st-browser-timings-grid',
  templateUrl: './browser-timings-grid.component.html',
  styleUrls: ['./browser-timings-grid.component.scss']
})
export class BrowserTimingsGridComponent implements OnInit {

  settings = {
    hideSubHeader: true,
    actions: {
      columnTitle: 'Actions',
      add: false,
      edit: false,
      delete: false,
      custom: [
        {
          name: 'viewSource',
          title: '<i class="far fa-eye grid-icon" style="font-size:0.9rem !important;"></i>'
        }
      ],
      
    },
    columns: {
      itemId: {
        title: 'Item Id',
        type: 'string',
      },
      iKey: {
        type: 'custom',
        title: 'Payload',
        renderComponent: JsonColumnComponent,
      },
      timeStamp: {
        title: 'Timestamp',
        type: 'string',
      },
      performanceBucket: {
        title: 'Performance Bucket',
        type: 'string',
      },
      url: {
        title: 'Url',
        type: 'string',
      }
    }
  };
  
  source: LocalDataSource = new LocalDataSource();
  dialogOpen: boolean = false;

  constructor(private zone: NgZone,
              public telemetryOverviewService: TelemetryOverviewService,
              private dialogService: NbDialogService) { }

  ngOnInit(): void {
    this.telemetryOverviewService.View$.subscribe(value => { 
      if (value === null || value.lastBrowserTimings === null) return;
      if (this.dialogOpen === true) return;

      this.zone.run(() => {

        if (value?.lastBrowserTimings !== null) {
          this.source.load(value.lastBrowserTimings)
        }
      })
    });
  }

  onViewSourceClick(event): void {
    switch ( event.action) {
      case 'viewSource':
        this.dialogOpen = true;

        this.dialogService.open(ViewSourceModalComponent, {
          context: {
            source: event.data,
          },
        })
        .onClose.subscribe(_ => this.dialogOpen = false);
       
        break;
    }
  }

}
