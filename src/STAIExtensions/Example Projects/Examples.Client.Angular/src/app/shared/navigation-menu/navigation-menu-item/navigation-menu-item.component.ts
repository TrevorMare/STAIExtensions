import { Component, Input, OnInit } from '@angular/core';
import { IMenuItem } from '../../../pages/pages-menu';

@Component({
  selector: 'st-navigation-menu-item',
  templateUrl: './navigation-menu-item.component.html',
  styleUrls: ['./navigation-menu-item.component.scss']
})
export class NavigationMenuItemComponent implements OnInit {

  @Input() menuItem: IMenuItem = {};

  constructor() { }

  ngOnInit(): void {
  }

}
