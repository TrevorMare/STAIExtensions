import { AfterContentInit, Component, ContentChildren, OnInit, QueryList } from '@angular/core';
import { TabItemComponent } from './tab-item/tab-item.component';

@Component({
  selector: 'st-tab-component',
  templateUrl: './tab-component.component.html',
  styleUrls: ['./tab-component.component.scss']
})
export class TabComponentComponent implements OnInit, AfterContentInit {

  @ContentChildren(TabItemComponent) tabs: QueryList<TabItemComponent>;
  
  constructor() { }
 

  ngOnInit(): void {
  }

  ngAfterContentInit(): void {
      let activeTabs = this.tabs.filter((tab)=>tab.active);
      if(activeTabs.length === 0) {
        this.selectTab(this.tabs.first);
      }
  }

  selectTab(tabItem: TabItemComponent){
    this.tabs.toArray().forEach(tab => tab.active = false);
    tabItem.active = true;
  }

}
