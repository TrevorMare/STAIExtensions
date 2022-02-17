import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AvailabilityAggregateGroup, AvailabilityOverviewView } from 'src/app/data/view.availability-overview';

@Component({
  selector: 'availabilitypage-availability-list',
  templateUrl: './availability-list.component.html',
  styleUrls: ['./availability-list.component.scss']
})
export class AvailabilityListComponent implements OnInit {
  
  _data: AvailabilityOverviewView | null = null;
  selectedItem: AvailabilityAggregateGroup | null = null;

  rootOpen: boolean = true;
  nameIndexOpen = new Map<number, boolean>();
  selectedFullGroupName: string = "";
  get data() :AvailabilityOverviewView | null { return this._data; }

  @Input() set data(value: AvailabilityOverviewView | null) { 
    this._data = value;
    this.initialiseSelectedItem();
  } 
  @Input() groupMinutes: number = 20;
  @Output() selectedItemChanged: EventEmitter<AvailabilityAggregateGroup | null> = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  private initialiseSelectedItem() {
    if (this._data?.aggregateGroup != null) {
      if (this.selectedFullGroupName == '') {
        this.setSelectedItem(this._data.aggregateGroup);
      } else {
        this.recurseSetSelectedItem(this._data.aggregateGroup);
      }
    }
  }

  private recurseSetSelectedItem(item: AvailabilityAggregateGroup) {
    if (item.fullGroupName === this.selectedFullGroupName) {
      this.setSelectedItem(item);
    } else {
      for (let child of item.children) {
        this.recurseSetSelectedItem(child);
      }
    }
  }

  public getNameGroupOpen(index: number): boolean {
    if (!this.nameIndexOpen.has(index)) {
      this.nameIndexOpen.set(index, false);
    }
    return this.nameIndexOpen.get(index) ?? false;
  }

  public toggleNameGroupOpen(index: number): void {
    var value = !this.getNameGroupOpen(index);
    this.nameIndexOpen.set(index, value);
  }

  public setSelectedItem(item: AvailabilityAggregateGroup | null): boolean {
    this.selectedItem = item;
    this.selectedItemChanged.emit(item);
    this.selectedFullGroupName = item?.fullGroupName ?? "";
    return false;
  }


}
