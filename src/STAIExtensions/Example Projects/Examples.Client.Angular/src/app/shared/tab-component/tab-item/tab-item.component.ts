import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'st-tab-item',
  templateUrl: './tab-item.component.html',
  styleUrls: ['./tab-item.component.scss']
})
export class TabItemComponent implements OnInit {

  constructor() { }
  @Input() title: string = "tab";
  @Input() active = false;
  
  ngOnInit(): void {
  }

}
