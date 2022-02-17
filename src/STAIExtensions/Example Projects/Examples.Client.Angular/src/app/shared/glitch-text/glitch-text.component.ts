import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { interval, timer } from 'rxjs';

@Component({
  selector: 'shared-glitch-text',
  templateUrl: './glitch-text.component.html',
  styleUrls: ['./glitch-text.component.scss']
})
export class GlitchTextComponent implements OnInit, AfterViewInit {

  timer = timer(Math.random() * 10000);
  intervalOn = timer(3000);
  intervalOff = timer(10000);
  isAnimationOn: boolean = false;
  
  constructor() { }

  @Input() text?: string = "";

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.timer.subscribe((val) => {
      this.showAnimation();
    });
  }

  private showAnimation() {
    this.isAnimationOn = true;
    this.intervalOn.subscribe((_) => {
      this.hideAnimation();
    });
  }

  private hideAnimation() {
    this.isAnimationOn = false;
    this.intervalOff.subscribe((_) => {
      this.showAnimation();
    });
  }

}
