import { Component, ContentChild, EventEmitter, Input, OnInit, Output, TemplateRef } from '@angular/core';
import * as uuid from 'uuid';

@Component({
  selector: 'shared-list-component',
  templateUrl: './list-component.component.html',
  styleUrls: ['./list-component.component.scss']
})
export class ListComponentComponent implements OnInit {

  public pageIndex: number = 1;
  pagerId: string = "";

  @ContentChild('itemTemplate', {read: TemplateRef, static: false }) itemTemplate: TemplateRef<any>;
  @ContentChild('headerTemplate', {read: TemplateRef, static: false }) headerTemplate: TemplateRef<any>;

  @Input() items: any[];
  @Input() hoverEnabled: boolean = true;
  @Input() isNestedList: boolean = false;
  @Input() noItemPadding: boolean = false;
  @Input() paginationEnabled: boolean = false;
  @Input() pageSize: number = 10;
  @Input() itemClass: string = "";
  @Output() currentPage: EventEmitter<number> = new EventEmitter();


  constructor() { 
    this.pagerId = 'pager-' + uuid.v4();
  }

  ngOnInit(): void {
  }

  public onPageChanged(index: number) {
    this.pageIndex = index;
    this.currentPage.next(index);
  }

}
