import { Component, Input, OnInit } from '@angular/core';
import { AvailabilityAggregateGroup } from 'src/app/data/view.availability-overview';


export type ListItem = {
  label: string;
  value: number | string;
  unit?: string;
}

@Component({
  selector: 'availabilitypage-details-panel',
  templateUrl: './details-panel.component.html',
  styleUrls: ['./details-panel.component.scss']
})
export class DetailsPanelComponent implements OnInit {
  
  _data: AvailabilityAggregateGroup | null = null;
  _fullGroupName: string = "";
  _displayIndex: number = 0;
  _numberOfItems: number = 0;
  items: ListItem[] = [];

  get data(): AvailabilityAggregateGroup | null { return this._data; }
  @Input() set data(value: AvailabilityAggregateGroup | null) {

    if (value !== undefined && value !== null ) {
      if (this._fullGroupName != value.fullGroupName) {
        this._displayIndex = 0;
        this._fullGroupName = value.fullGroupName;
        this._numberOfItems = value.items.length;
      }
    }
    this._data = value;
    this.buildItems();
  }
 
  constructor() { }

  ngOnInit(): void {
  }


  public navigateNext(): void {
    if (this._displayIndex === this._numberOfItems - 1) return;
    this._displayIndex++;
    this.buildItems();
  }

  public navigatePrevious(): void {
    if (this._displayIndex === 0) return;
    this._displayIndex--;
    this.buildItems();
  }

  private buildItems(): void {
    this.items = [];

    if (this._data != null) {
      var buildFromSource = this._data.items[this._displayIndex];
      

      this.items.push({ label: 'Average Duration', value : buildFromSource.averageDuration.round(0), unit: " ms" });
      this.items.push({ label: 'Minimum Duration', value : buildFromSource.minDuration, unit: " ms" });
      this.items.push({ label: 'Maximum Duration', value : buildFromSource.maxDuration, unit: " ms" });
      this.items.push({ label: 'Success Count', value : buildFromSource.successfulCount });
      this.items.push({ label: 'Failed Count', value : buildFromSource.failureCount });
      this.items.push({ label: 'From Date', value : buildFromSource.startDate.toString() });
      this.items.push({ label: 'To Date', value : buildFromSource.endDate.toString() });

    }

  }

}
