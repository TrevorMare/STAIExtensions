import { Component, Input, OnInit } from '@angular/core';
import { GroupValues } from 'src/app/data/view.browsertimings-overview';

@Component({
  selector: 'browsertimingpage-group-value-display',
  templateUrl: './group-value-display.component.html',
  styleUrls: ['./group-value-display.component.scss']
})
export class GroupValueDisplayComponent implements OnInit {


  @Input() groupValues: GroupValues | null;
  @Input() groupValueName: string = "";

  constructor() { }

  ngOnInit(): void {
    //this.groupValues?.statistics
  }

}
