import { Component, NgZone, OnInit } from '@angular/core';
import { NbDialogService } from "@nebular/theme";
import { JsonColumnComponent } from '../../json-column/json-column.component';
import { DataContractGridComponent } from '../datacontract-grid.component';

@Component({
  selector: 'stgrid-datacontract-custom-metrics',
  templateUrl: './datacontract-grid-custom-metrics.component.html',
  styleUrls: ['./datacontract-grid-custom-metrics.component.scss']
})
export class DataContractGridCustomMetricsComponent extends DataContractGridComponent implements OnInit {

  tableSettings = {
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
      name: {
        title: 'Name',
        type: 'string',
      },
      value: {
        title: 'Value',
        type: 'number',
      },
      valueCount: {
        title: 'Value Count',
        type: 'number',
      },
      valueMax: {
        title: 'Value Max',
        type: 'number',
      },
      valueMin: {
        title: 'Value Min',
        type: 'number',
      }
    }
  };

  constructor(zone: NgZone,
              dialogService: NbDialogService) { 
    super(zone, dialogService);
  }


}

