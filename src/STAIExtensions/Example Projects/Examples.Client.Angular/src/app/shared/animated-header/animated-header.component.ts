import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
declare const animateHeader: any;

@Component({
  selector: 'shared-animated-header',
  templateUrl: './animated-header.component.html',
  styleUrls: ['./animated-header.component.scss']
})
export class AnimatedHeaderComponent implements OnInit, AfterViewInit  {

  @ViewChild('textElement') textElement: ElementRef<any>; 
  @Input() text: string = "";

  constructor() { }
  ngAfterViewInit(): void {
    animateHeader(this.textElement.nativeElement, this.text);
  }

  ngOnInit(): void {
    
  }

}
