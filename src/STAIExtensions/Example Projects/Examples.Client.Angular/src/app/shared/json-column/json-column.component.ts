import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'st-json-column',
  templateUrl: './json-column.component.html',
  styleUrls: ['./json-column.component.scss']
})
export class JsonColumnComponent implements OnInit {

  @Input() rowData: any;

  constructor() { }

  ngOnInit(): void {
  }

  getJsonData(): string {

    if (this.rowData === null) return "null";
    return JSON.stringify(this.rowData);

  }

}
