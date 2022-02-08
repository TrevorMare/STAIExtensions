import { AfterViewInit, Component, OnInit } from '@angular/core';
declare const notFoundAnimation: any;

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss']
})
export class NotFoundComponent implements OnInit, AfterViewInit  {

  constructor() { }
  ngAfterViewInit(): void {
    notFoundAnimation();
  }

  ngOnInit(): void {
  }

}
