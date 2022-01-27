import { Input, NgZone, Component, OnInit } from "@angular/core";
import { NbDialogService } from "@nebular/theme";
import { LocalDataSource } from "ng2-smart-table";
import { ViewSourceModalComponent } from "../view-source-modal/view-source-modal.component";

@Component({ template: '' })
export abstract class DataContractGridComponent implements OnInit {
    
    dialogOpen: boolean = false;
    dataGridSource: LocalDataSource = new LocalDataSource();
    initialised: boolean = false;
    abstract tableSettings: any;

    private _dataContractItems: any = null;
    get dataContractItems(): any {
        return this._dataContractItems;
    }

    @Input() set dataContractItems(value: any) {
        if (value === undefined) value = null;
        this._dataContractItems = value;
        
        this.BindTableData();
    }

    ngOnInit(): void {
      this.initialised = true;
      this.BindTableData();
    }

    constructor(protected zone: NgZone,
                protected dialogService: NbDialogService) {

    }

    private BindTableData(): void {
      if (this.initialised === false) return;
      this.zone.run(() => {
        if (this._dataContractItems !== null) {
            this.dataGridSource.load(this._dataContractItems)
        }
      })
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