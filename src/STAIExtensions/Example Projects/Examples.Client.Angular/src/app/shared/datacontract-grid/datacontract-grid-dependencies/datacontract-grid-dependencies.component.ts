import { Component, NgZone, OnInit } from '@angular/core';
import { NbDialogService } from "@nebular/theme";
import { JsonColumnComponent } from '../../json-column/json-column.component';
import { DataContractGridComponent } from '../datacontract-grid.component';

@Component({
  selector: 'stgrid-datacontract-dependencies',
  templateUrl: './datacontract-grid-dependencies.component.html',
  styleUrls: ['./datacontract-grid-dependencies.component.scss']
})
export class DataContractGridDependenciesComponent extends DataContractGridComponent implements OnInit {

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
      name: {
        title: 'Name',
        type: 'string',
      },
      duration: {
        title: 'Duration (ms)',
        type: 'number',
      },
      timeStamp: {
        title: 'Timestamp',
        type: 'string',
      }
    }
  };
  
  constructor(zone: NgZone,
              dialogService: NbDialogService) { 
    super(zone, dialogService);
  }


}