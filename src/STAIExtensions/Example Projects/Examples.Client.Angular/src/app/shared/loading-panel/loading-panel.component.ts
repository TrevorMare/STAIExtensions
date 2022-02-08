import { AfterViewInit, Component, OnInit } from '@angular/core';
declare const animateCircle: any;

@Component({
  selector: 'st-loading-panel',
  templateUrl: './loading-panel.component.html',
  styleUrls: ['./loading-panel.component.scss']
})
export class LoadingPanelComponent implements OnInit, AfterViewInit {

  constructor() { }
  
  ngAfterViewInit(): void {
    animateCircle();
  }

  ngOnInit(): void {
  }

}
