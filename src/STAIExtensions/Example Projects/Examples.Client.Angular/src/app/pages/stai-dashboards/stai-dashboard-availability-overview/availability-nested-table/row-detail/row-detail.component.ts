import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AvailabilityAggregateGroup } from '../../../../../@core/data/availability-overview';

@Component({
  selector: 'st-row-detail',
  templateUrl: './row-detail.component.html',
  styleUrls: ['./row-detail.component.scss']
})
export class RowDetailComponent implements OnInit {

  constructor() { }

  @Input() isMainRow: boolean = false;
  @Input() isExpanded: boolean = false;
  @Input() isSelected: boolean = false;
  @Input() dataSource: AvailabilityAggregateGroup = null;
  @Input() level: number = 0;

  @Output() onSelected = new EventEmitter<any>();


  ngOnInit(): void {
  }

}
