import { Component, Input, OnInit } from '@angular/core';
import { IMenuItem, MENU_ITEMS } from 'src/app/pages/pages-menu';

@Component({
  selector: 'shared-navigation-menu',
  templateUrl: './navigation-menu.component.html',
  styleUrls: ['./navigation-menu.component.scss']
})
export class NavigationMenuComponent implements OnInit {

  constructor() { }

  menuItems: IMenuItem[] = MENU_ITEMS;
  @Input() isOpen: boolean = true;

  ngOnInit(): void {
  }

}
