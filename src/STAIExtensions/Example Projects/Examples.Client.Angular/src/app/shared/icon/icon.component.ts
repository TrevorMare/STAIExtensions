import { Component, Input, OnInit } from '@angular/core';
import { ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'st-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class IconComponent implements OnInit {

  constructor() { }

  _baseClassName: string = "icon";

  @Input() iconClass?: string = "";
  @Input() addClass?: string = "";
  @Input() iconSet?: string = "font-awesome";

  ngOnInit(): void {
  }

  public getClassList(): string[] {
    const result: string[] = [ this._baseClassName ];

    if (this.iconSet === 'font-awesome' || this.iconSet === 'boxicons') {
      if (this.iconClass !== null && this.iconClass !== '') {
        this.iconClass?.split(' ').forEach((className) => { result.push(className); });
      }
    } else if (this.iconSet === 'material-icons') {
      result.push('material-icons');
    }

    if (this.addClass !== null && this.addClass !== '') {
      this.addClass?.split(' ').forEach((className) => { result.push(className); });
    }
    return result;
  }

}
