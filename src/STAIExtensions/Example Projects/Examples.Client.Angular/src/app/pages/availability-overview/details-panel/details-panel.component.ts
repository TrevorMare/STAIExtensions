import { Component, Input, OnInit } from '@angular/core';
import { AvailabilityAggregateGroup } from 'src/app/data/view.availability-overview';

@Component({
  selector: 'st-details-panel',
  templateUrl: './details-panel.component.html',
  styleUrls: ['./details-panel.component.scss']
})
export class DetailsPanelComponent implements OnInit {
  _data: AvailabilityAggregateGroup | null = null;
  get data(): AvailabilityAggregateGroup | null { return this._data; }
  @Input() set data(value: AvailabilityAggregateGroup | null) {
    this._data = value;
  }
  
  constructor() { }

  ngOnInit(): void {
  }

}
