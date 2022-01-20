import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'st-number-card',
  templateUrl: './number-card.component.html',
  styleUrls: ['./number-card.component.scss']
})
export class NumberCardComponent implements OnInit {

  
  @Input() total: number = 0;
  @Input() cardLabel: string = '';
  @Input() cardIcon: string = '';

  constructor() { }

  ngOnInit(): void {
  }

}
