import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'shared-augmented-card',
  templateUrl: './augmented-card.component.html',
  styleUrls: ['./augmented-card.component.scss']
})
export class AugmentedCardComponent implements OnInit {

  constructor() { }
  
  _augBorderStyle1: string = "tl-2-clip-x tr-2-rect r-clip-y br-clip bl-clip-y border";
  _augBorderStyle2: string = "tl-rect tr-2-rect-x bl-2-clip-xy border";
  _augBorderStyle3: string = "tl-2-clip-y tr-2-clip-x br-clip-inset border";

  @Input() selected: boolean = false;
  @Input() borderStyle: number = 1;
  @Input() colorStyle: number = 1;
  @Input() showOver: boolean = true;
  @Input() cardClass: string = '';

  ngOnInit(): void {
  }

  public GetBorderStyle() {
    if (this.borderStyle == 2) {
      return this._augBorderStyle2;
    } else if (this.borderStyle == 3) {
      return this._augBorderStyle3;
    }
    return this._augBorderStyle1;
  }

}
