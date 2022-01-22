import { Component, NgZone, OnInit } from '@angular/core';
import { LocalDataSource } from 'ng2-smart-table';
import { NbDialogService } from '@nebular/theme';
import { TelemetryOverviewService } from '../../@core/utils/telemetry-overview.service';
import { ViewSourceModalComponent } from '../view-source-modal/view-source-modal.component';

@Component({
  selector: 'st-availability-grid',
  templateUrl: './availability-grid.component.html',
  styleUrls: ['./availability-grid.component.scss']
})
export class AvailabilityGridComponent implements OnInit {

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
          title: '<i class="nb-close inline-block width: 50px"></i>'
        }
      ],
      position: 'left'
    },
    columns: {
      itemId: {
        title: 'Item Id',
        type: 'string',
      },
      cloudRoleInstance: {
        title: 'Cloud Role Instance',
        type: 'string',
      },
      cloudRoleName: {
        title: 'Cloud Role Name',
        type: 'string',
      },
      timeStamp: {
        title: 'Timestamp',
        type: 'string',
      },
      performanceBucket: {
        title: 'Performance Bucket',
        type: 'string',
      },
      success: {
        title: 'Success',
        type: 'string',
      },
      size: {
        title: 'size',
        type: 'number',
      }
    }
  };
  
  source: LocalDataSource = new LocalDataSource();
  dialogOpen: boolean = false;

  constructor(private zone: NgZone,
              public telemetryOverviewService: TelemetryOverviewService,
              private dialogService: NbDialogService) { 
  }

  ngOnInit(): void {
    this.telemetryOverviewService.View$.subscribe(value => { 
      if (value === null || value.lastAvailability === null) return;
      if (this.dialogOpen === true) return;

      this.zone.run(() => {

        if (value?.lastAvailability !== null) {
          this.source.load(value.lastAvailability)
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
