import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'st-toggle-card',
  templateUrl: './toggle-card.component.html',
  styleUrls: ['./toggle-card.component.scss']
})
export class ToggleCardComponent implements OnInit {

  constructor() { }

  @Input() isOpen: boolean = false;
  @Input() title: string = "";

  ngOnInit(): void {
  }

}
