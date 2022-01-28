import { Component, Input, OnInit } from '@angular/core';
import { AvailabilityAggregateGroup } from '../../../../@core/data/availability-overview';

@Component({
  selector: 'st-availability-nested-table',
  templateUrl: './availability-nested-table.component.html',
  styleUrls: ['./availability-nested-table.component.scss']
})
export class AvailabilityNestedTableComponent implements OnInit {

  private _dataSource: AvailabilityAggregateGroup;

  get dataSource(): AvailabilityAggregateGroup {
    return this._dataSource;
  }

  @Input() set dataSource(value: AvailabilityAggregateGroup) {
    this._dataSource = value;
    console.log(`Setting data source`);
  }

  constructor() { }

  ngOnInit(): void {
  }

}
