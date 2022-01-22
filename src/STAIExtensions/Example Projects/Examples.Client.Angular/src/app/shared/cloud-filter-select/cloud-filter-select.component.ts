import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Component({
  selector: 'ts-cloud-filter-select',
  templateUrl: './cloud-filter-select.component.html',
  styleUrls: ['./cloud-filter-select.component.scss']
})
export class CloudFilterSelectComponent implements OnInit, OnChanges  {

  @Output() filterChanged: EventEmitter<any> = new EventEmitter();
  @Output() selectedFilterChanged: EventEmitter<string[]> = new EventEmitter();
  @Input() cloudNames: Record<string, string[]>;
  @Input() debounceTime: number = 2000;
  @Input() selectedItems: string[] = [ '-1' ];

  availableCloudFilters$ = new BehaviorSubject<string[]>(null);
  selectedFilterDebounce$ = new Subject<any>();
  

  constructor() { 
    this.selectedFilterDebounce$.debounceTime(this.debounceTime).subscribe(() => {
      this.BuildApplyFilterObject();
    });
  }

  ngOnChanges(): void {
    this.BuildFilterNames();
  }

  ngOnInit(): void {
    this.BuildFilterNames();
  }

  onCloudFilterOptionSelectChanged(event$: any) {
    this.selectedFilterDebounce$.next(this.selectedItems);
  }
  
  private BuildFilterNames(): void {
    const cloundInstances = this.cloudNames;
    if (!!cloundInstances) {
      const filterNames: string[] = new Array();
      Object.keys(cloundInstances).forEach(key => {
        const roleNames = cloundInstances[key] as string[];
        roleNames?.forEach(roleName => {
          var displayInstanceName = (key === '') ? 'None' : key;
          var displayRoleName = (roleName === '') ? 'None' : roleName;
          var option = `${displayInstanceName} - ${displayRoleName}`;
          filterNames.push(option);
        });

        this.availableCloudFilters$.next(filterNames);
      });
    } else {
      this.availableCloudFilters$.next(null);
    }
  }

  private BuildApplyFilterObject(): void {
    if (this.selectedItems.length === 0 || this.selectedItems.indexOf('-1') >= 0) {
      this.filterChanged.emit(null);
      this.selectedFilterChanged.emit(["-1"])
    } else {

      var roleInstances: string[] = new Array();
      var roleNames: string[] = new Array();

      this.selectedItems.forEach(item => {
        const index = parseInt(item);
        const selectedItem = this.availableCloudFilters$.value[index];

        const parts = selectedItem.split(' - ');
        if (parts[0] === 'None') {
          roleInstances.push("");
        } else {
          roleInstances.push(parts[0])
        }

        if (parts[1] === 'None') {
          roleNames.push("");
        } else {
          roleNames.push(parts[1])
        }
      });

      const parameters = {
        "CloudRoleInstance": [
          roleInstances
        ],
        "CloudRoleName": [
          roleNames
        ]
      };
      this.filterChanged.emit(parameters);
      this.selectedFilterChanged.emit(this.selectedItems)
    }
  }


}
